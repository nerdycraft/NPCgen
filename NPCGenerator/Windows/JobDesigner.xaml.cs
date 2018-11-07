using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using Newtonsoft.Json;

using NPCGenerator.Model;
using NPCGenerator.Util;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class JobDesigner
    {
        private readonly JobDesignerVM vm = new JobDesignerVM {NewMode = true, EditMode = true};
        public JobDesigner(IEnumerable<Job> jobs, IEnumerable<Talent> talents)
        {
            InitializeComponent();
            vm.Talents = talents;
            foreach (var job in jobs)
                vm.Jobs.Add(job);

            DataContext = vm;
        }

        private void OnAddJobClick(object sender, RoutedEventArgs e)
        {
            var dummyJob = vm.Jobs.FirstOrDefault(job => job.IsNew);
            if (dummyJob == null)
            {
                dummyJob = new Job {IsNew = true};
                vm.Jobs.Add(dummyJob);
            }

            List.SelectedItem = dummyJob;
        }

        private void OnSaveJobClicked(object sender, RoutedEventArgs e)
        {
            if (List.SelectedItem is Job job)
            {
                if (string.IsNullOrEmpty(job.ReferenceName)) //no empty id
                {
                    MessageBox.Show("Bitte setzen Sie eine ID für diesen Job.");
                    return;
                }

                if (job.IsNew && vm.Jobs.FirstOrDefault(j => string.Equals(job.ReferenceName, j.ReferenceName, StringComparison.CurrentCultureIgnoreCase)) != null) //no duplicate id
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
                File.WriteAllText(Path.Combine(Controller.JOB_FOLDER, $"{job.ReferenceName.ToLower()}.json"), JsonConvert.SerializeObject(job));
            }
        }

        private void OnDeleteJobClicked(object sender, RoutedEventArgs e)
        {
            if (List.SelectedItem is Job job)
            {
                try
                {
                    File.Delete(Path.Combine(Controller.JOB_FOLDER, $"{job.ReferenceName.ToLower()}.json"));
                    vm.Jobs.Remove(job);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}
