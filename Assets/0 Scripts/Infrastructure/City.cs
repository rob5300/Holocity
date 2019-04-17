using System.Collections.Generic;
using Infrastructure.Grid;
using UnityEngine;
using Infrastructure.Residents;
using Infrastructure.Grid.Entities.Buildings;
using Settings;
using Settings.Adjustment;
using System;

namespace Infrastructure {
    public enum TimePeriod { Modern, Future, Past }

    public class City {
        public string Owner;
        public Session ParentSession;
        /// <summary>
        /// How many residents can fill a vacant building per update.
        /// </summary>
        public int FillLimitPerUpdate = 1;

        private List<Resident> Residents;
        protected List<GridSystem> CityGrids = new List<GridSystem>();
        public float TotalHappinessAverage = 0;
        public TimePeriod CurrentTimePeriod = TimePeriod.Modern;

        private List<Residential> _residentialBuildings;
        private List<Residential> _vacantResidentialBuildings;
        private List<Resident> _homelessResidents;

        private List<Job> _avaliableJobs;
        private List<Resident> _unemployedResidents;

        private AdjustableFloat ResidentialIncreaseRate;
        private ThresholdAdjusterFloat ResidentialIncreaseAdjuster;

        private float _residentUpdateTime;

        public City(string owner, Session sess)
        {
            owner = owner != string.Empty ? owner : "Mayor";
            ParentSession = sess;

            //Starting Residential Demand
            ResidentialIncreaseRate = ParentSession.Settings.ResidentialDemandIncreaseRate;
            ResidentialIncreaseAdjuster = (ThresholdAdjusterFloat)ResidentialIncreaseRate.Adjuster;

            Residents = new List<Resident>(Mathf.CeilToInt(ParentSession.Settings.ResidentialDemand));
            _homelessResidents = new List<Resident>();
            _residentialBuildings = new List<Residential>(Mathf.CeilToInt(ParentSession.Settings.ResidentialDemand));
            _vacantResidentialBuildings = new List<Residential>(Mathf.CeilToInt(ParentSession.Settings.ResidentialDemand));

            _avaliableJobs = new List<Job>();
            _unemployedResidents = new List<Resident>();
        }

        /// <summary>
        /// Used to setup various components once all others are created.
        /// </summary>
        public void PostSetup()
        {
            ParentSession.TickManager.PostTick += ResetResourceTickCounters_Event;
            ParentSession.TickManager.PreTick += DemandUpdate_Event;
            ParentSession.TickManager.PreTick += ResidentVacancyUpdate_Event;
            ParentSession.TickManager.PreTick += ResidentJobUpdate_Event;
            ParentSession.TickManager.PreTick += ResidentHappinessUpdate_Event;
            ParentSession.TickManager.LowPriorityTick += ResidentHappinessUpdate_Event;
        }

        public bool CreateGrid(int width, int height, Vector3 worldGridPosition)
        {
            GridSystem newGrid = new GridSystem(width, height, CityGrids.Count, this, ParentSession.TickManager, worldGridPosition);
            CityGrids.Add(newGrid);
            newGrid.OnNewResidentialBuilding += NewResidential_Event;
            newGrid.OnNewCommercialBuilding += NewCommercial_Event;
            return true;
        }

        public void ProcessHomelessResident(Resident res)
        {
            _homelessResidents.Add(res);
            _avaliableJobs.Add(res.Job);
            res.Job.Taken = false;
            res.RemoveJob();
        }

        public void ProcessDestroyedCommercial(Commercial com)
        {
            foreach(Job job in com.Jobs)
            {
                if (_avaliableJobs.Contains(job)) _avaliableJobs.Remove(job);
            }
        }

        public void ProcessResidentialAsVacant(Residential residentialNowVacant)
        {
            if (_residentialBuildings.Contains(residentialNowVacant))
            {
                _residentialBuildings.Remove(residentialNowVacant);
                _vacantResidentialBuildings.Add(residentialNowVacant);
            }
        }

        private void RecalculateFillLimit()
        {
            FillLimitPerUpdate = Mathf.CeilToInt(_residentialBuildings.Count * 0.1f);
        }

        public GridSystem GetGrid(int id)
        {
            if (CityGrids.Count > id) return CityGrids[id];
            return null;
        }

        public List<GridSystem> GetGrids()
        {
            return CityGrids;
        }

        #region Event Handlers

        /// <summary>
        /// Update the happiness average on each grid system and the total city wide level.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="tickTime"></param>
        private void ResidentHappinessUpdate_Event(Session s, float tickTime)
        {
            float totalAverage = 0;
            //Update the happiness totals for each grid.
            foreach(GridSystem grid in CityGrids)
            {
                totalAverage += grid.UpdateHappiness();
            }
            //Get the average for all grid system happiness averages.
            TotalHappinessAverage = totalAverage / CityGrids.Count + 1;
        }

        private void DemandUpdate_Event(Session s, float tickTime)
        {
            //Update the values for the residential increase amount adjuster
            //Input the percent of the current resident demand vs the total residents we have
            ResidentialIncreaseAdjuster.InputValue = Residents.Count > 0 ? Residents.Count / ParentSession.Settings.ResidentialDemand : 0;
        }

        private void ResetResourceTickCounters_Event(Session s, float time)
        {
            foreach(GridSystem grid in CityGrids)
            {
                grid.ResetResourceTickCounters();
            }
        }
        
        private void NewResidential_Event(Residential res)
        {
            if (!_residentialBuildings.Contains(res))
            {
                //If the building is Vacant, we put it in the vacant buildings list.
                //We need to sort everything now at the expense of memory.
                //Cant afford to check all buildings each time or use LINQ ;(.
                if (res.IsVacant)   _vacantResidentialBuildings.Add(res);
                else                _residentialBuildings.Add(res);
            }
        }

        private void NewCommercial_Event(Commercial obj)
        {
            _avaliableJobs.AddRange(obj.Jobs);
        }

        /// <summary>
        /// Checks if there is any avaliable residential properties to move residents into
        /// </summary>
        private void ResidentVacancyUpdate_Event(Session s, float time)
        {
            if (_vacantResidentialBuildings.Count == 0) return;
            _residentUpdateTime += time;
            //If we have waited 0.1 seconds run.
            if(_residentUpdateTime > 0.25)
            {
                if(_vacantResidentialBuildings.Count > 0 && ParentSession.Settings.ResidentialDemand > 0)
                {
                    time = 0;
                    for (int i = _vacantResidentialBuildings.Count - 1; i >= 0; i--)
                    {
                        //Go through the vacant buildings and add the residents to them.
                        _vacantResidentialBuildings[i].SetResident(new Resident());
                        //This resident has a home and can now have a job
                        _unemployedResidents.Add(_vacantResidentialBuildings[i].Resident);
                        //Enqueue the new resident in the tick manager as low priority.
                        ParentSession.TickManager.LowPriorityIncomingQueue.Enqueue(_vacantResidentialBuildings[i].Resident);
                        //Add the residential building to the list as it is now holding a resident.
                        _residentialBuildings.Add(_vacantResidentialBuildings[i]);
                        _vacantResidentialBuildings.RemoveAt(i);
                        ParentSession.Settings.ResidentialDemand--;

                        //Check if we should increase the fill limit incase the vacant bulding amount has increased to make the fill rate very slow
                        if(FillLimitPerUpdate < Mathf.CeilToInt(_residentialBuildings.Count * 0.5f))
                        {
                            RecalculateFillLimit();
                        }

                        //If the demand is now empty or if the fill limit was reached, stop.
                        if (ParentSession.Settings.ResidentialDemand < 1 || i+1 > FillLimitPerUpdate) return;
                    }
                }
                else
                {
                    //There are no vacant buildings. Update the fill rate using the current number of residential buildings that are populated.
                    RecalculateFillLimit();
                }
            }
        }

        private void ResidentJobUpdate_Event(Session s, float time)
        {
            if (_avaliableJobs.Count == 0) return;

            for (int i = 0; i < _unemployedResidents.Count; i++)
            {
                //Give jobs to each unemployed resident until we have no jobs left.
                _unemployedResidents[i].SetJob(_avaliableJobs[0]);
                _avaliableJobs.RemoveAt(0);
                if (_avaliableJobs.Count == 0) return;
            }
        }
        #endregion
    }
}
