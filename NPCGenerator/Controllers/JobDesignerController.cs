using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Newtonsoft.Json;

using NPCGenerator.Model;
using NPCGenerator.Util;
using NPCGenerator.ViewModels;
using NPCGenerator.Windows;

namespace NPCGenerator.Controllers
{
    public class JobDesignerController
    {
        public DataContainer Data { get; }

        public JobDesignerController(DataContainer data) { Data = data; }

        public void Run()
        {
            var jd = new JobDesigner(new JobDesignerVM { Jobs = Data.Jobs, Talents = Data.Talents });
            jd.AddClicked += AddJob;
            jd.SaveClicked += (sender, job) => SaveJob(job);
            jd.DeleteClicked += (sender, job) => DeleteJob(job);
            jd.Closed += delegate { CleanJobs(); };
            jd.ShowDialog();
        }

        private Job AddJob()
        {
            var dummyJob = Data.Jobs.FirstOrDefault(job => job.IsNew);
            if (dummyJob == null)
            {
                dummyJob = new Job { IsNew = true };
                Data.Jobs.Add(dummyJob);
            }

            return dummyJob;
        }

        private void SaveJob(Job job)
        {
            if (string.IsNullOrEmpty(job.ReferenceName)) //no empty id
            {
                MessageBox.Show("Bitte setzen Sie eine ID für diesen Job.");
                return;
            }
            if (string.IsNullOrEmpty(job.Name) || string.IsNullOrEmpty(job.FemName))
            {
                MessageBox.Show("Bitte geben sie dem Job einen Namen.");
                return;
            }
            if (job.IsNew && Data.Jobs.FirstOrDefault(j => j != job && string.Equals(job.ReferenceName, j.ReferenceName, StringComparison.CurrentCultureIgnoreCase)) != null) //no duplicate id
            {
                MessageBox.Show("Die gesetzte ID wird bereits verwendet.");
                return;
            }
            if (job.Statweight.CumKk != 100) //komuliert
            {
                MessageBox.Show("Das komulierte Ergebnis der Attribut-Gewichtung muss 100 ergeben.");
                return;
            }
            if (job.Talents.Sum(t => t.Weight) > 800)
            {
                MessageBox.Show("Das komulierte Ergebnis der Talent-Gewichtung darf maximal 800 ergeben.");
                return;
            }

            job.IsNew = false;
            File.WriteAllText(Path.Combine(References.JOB_FOLDER, $"{job.ReferenceName.ToLower()}.json"), JsonConvert.SerializeObject(job));
        }

        private void DeleteJob(Job job)
        {
            try
            {
                File.Delete(Path.Combine(References.JOB_FOLDER, $"{job.ReferenceName.ToLower()}.json"));
                Data.Jobs.Remove(job);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CleanJobs()
        {
            Job job;
            while ((job = Data.Jobs.FirstOrDefault(j => j.IsNew)) != null)
                Data.Jobs.Remove(job);
        }
    }
}