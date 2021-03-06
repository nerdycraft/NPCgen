﻿using System;
using System.Collections.Generic;
using System.Linq;

using NPCGenerator.Dto;

// ReSharper disable PossibleMultipleEnumeration

namespace NPCGenerator.Util
{
    public class Generator
    {
        private const int BUILD_VERSION = 1;

        private readonly Random rnd = new Random();
        private readonly DataContainer data;

        public DataRow RowXp { get; set; }
        public DataRow RowFirstName { get; set; }
        public DataRow RowLastName { get; set; }
        public DataRow RowAge { get; set; }
        public DataRow RowGender { get; set; }
        public DataRow RowSpecies { get; set; }
        public DataRow RowCulture { get; set; }
        public DataRow RowStature { get; set; }
        public DataRow RowSize { get; set; }
        public DataRow RowCharacter { get; set; }
        public DataRow RowJob { get; set; }

        public event EventHandler<string> UpdateStatus;

        protected virtual void OnUpdateStatus(string e) { UpdateStatus?.Invoke(this, e); }

        public Generator(DataContainer data) { this.data = data; }
        public NPC Generate()
        {
            OnUpdateStatus("Erstelle NPC!");
            var npc = new NPC
                      {
                          Build = BUILD_VERSION,

                          Age = RowAge.NoGeneration ? RowAge.Value : (uint)rnd.Next(12, 100),
                          Size = RowSize.NoGeneration ? RowSize.Value : data.Sizes.GetRandom(),
                          Stature = RowStature.NoGeneration ? RowStature.Value : data.Statures.GetRandom(),
                          Charakter = RowCharacter.Value
                      };

            Level level = RowXp.Value;
            npc.Level = level.Name;

            Gender gender = RowGender.NoGeneration ? RowGender.Value : data.Gender.GetRandom();
            npc.Gender = gender.Id;

            OnUpdateStatus("Entdecke Spezies.");
            Species species = RowSpecies.NoGeneration ? RowSpecies.Value : data.Species.GetRandom();
            npc.Species = species.Id;

            OnUpdateStatus("Erforsche Kultur.");
            var culture = GetCulture(species);
            npc.Culture = culture.Name;

            OnUpdateStatus("Einen neuen Job antreten.");
            var job = GetJob(culture);
            npc.Job = gender.NameList == Gender.NamingList.Female ? job.FemName : job.Name;

            npc.Name = GetName(gender.NameList, culture);

            OnUpdateStatus("Neue Talente erforschen.");
            npc.Talents = CalculateTalents(level, MergeBaseTalents(culture.Talents, job.Talents)).ToArray();

            npc.Attributes = CalculateAttr(level, job.Statweight, species.Mods);
            npc.Stats = CalculateStats(species, npc.Attributes);
            OnUpdateStatus($"NPC Erstellt: {npc}");
            return npc;
        }

        private Attributes CalculateAttr(Level level, Statweight weight, IEnumerable<AttrMod> mods)
        {
            if (weight.CumKk != 100)
                throw new GenerationException("Ich habe gesagt es muss 100 ergeben!!!!");

            var attr = new Attributes();

            var maxAttr = CalcMaxAttr(mods);

            var pointsSpent = 0;
            while (pointsSpent < level.Attr)
            {
                var w = rnd.Next(1, 100);
                if (w <= weight.CumMu && attr.Mu < level.MaxAttr + maxAttr["MU"])
                    attr.Mu++;
                else if (w <= weight.CumKl && attr.Kl < level.MaxAttr + maxAttr["KL"])
                    attr.Kl++;
                else if (w <= weight.CumIn && attr.In < level.MaxAttr + maxAttr["IN"])
                    attr.In++;
                else if (w <= weight.CumCh && attr.Ch < level.MaxAttr + maxAttr["CH"])
                    attr.Ch++;
                else if (w <= weight.CumFf && attr.Ff < level.MaxAttr + maxAttr["FF"])
                    attr.Ff++;
                else if (w <= weight.CumGe && attr.Ge < level.MaxAttr + maxAttr["GE"])
                    attr.Ge++;
                else if (w <= weight.CumKo && attr.Ko < level.MaxAttr + maxAttr["KO"])
                    attr.Ko++;
                else if (w <= weight.CumKk && attr.Kk < level.MaxAttr + maxAttr["KK"])
                    attr.Kk++;
                else
                    continue;
                pointsSpent++;
            }

            return attr;
        }

        private IEnumerable<Talent> CalculateTalents(Level level, IEnumerable<Talent> talents)
        {
            var talentWeight = new Dictionary<int, Talent>();
            var noWeightTalents = 0;
            var totalWeight = 0;
            foreach (var talent in talents)
            {
                if (talent.Weight > 0)
                {
                    totalWeight += talent.Weight;
                    talentWeight.Add(totalWeight, talent);
                }
                else if (talent.Weight == 0)
                    noWeightTalents++;
            }

            if (totalWeight > 800)
                throw new GenerationException("Job Talent weight should be 800!");

            var newTalentWeight = (int)Math.Ceiling((1000f - totalWeight) / noWeightTalents);
            foreach (var talent in talents.Where(t => t.Weight == 0))
            {
                totalWeight += newTalentWeight;
                talentWeight.Add(totalWeight, talent);
            }

            //punkte verteilen
            var pointsSpent = 0;
            while (pointsSpent < level.Fw)
            {
                var rndWeight = rnd.Next(1, totalWeight);
                foreach (var kvp in talentWeight)
                    if (kvp.Key >= rndWeight)
                    {
                        if (kvp.Value.Value >= level.MaxFw)
                            break;

                        kvp.Value.Value++;
                        pointsSpent++;
                    }
            }

            return talentWeight.Values;
        }

        private IEnumerable<Talent> MergeBaseTalents(IEnumerable<Talent> cultTalents, IEnumerable<Talent> jobTalents)
        {
            var talents = data.Talents.ToList();

            foreach (var talent in talents)
            {
                var tal = cultTalents.FirstOrDefault(t => t.Id == talent.Id);
                if (tal != null)
                {
                    talent.Value += tal.Value;
                    talent.Weight = 0;
                }

                tal = jobTalents.FirstOrDefault(t => t.Id == talent.Id);
                if (tal != null)
                {
                    talent.Value += tal.Value;
                    talent.Weight = tal.Weight;
                }
            }

            return talents.Where(talent => !talent.Ignore);
        }

        private Culture GetCulture(Species species)
        {
            Culture culture;
            if (RowCulture.NoGeneration)
                culture = RowCulture.Value;
            else
            {
                var defaultCults = data.Cultures.Where(c => c.DefaultSpecies.Contains(species.Id));
                if (!defaultCults.Any())
                    throw new GenerationException("there is no culture with the species " + species.Name);
                culture = defaultCults.GetRandom();
            }

            return culture;
        }

        private Job GetJob(Culture culture)
        {
            return RowJob.NoGeneration ? (Job)RowJob.Value : data.Jobs.Where(j => culture.DefaultJobs.Contains(j.ReferenceName)).GetRandom();
        }

        private string GetName(Gender.NamingList naming, Culture culture)
        {
            string firstName = string.Empty, lastName;

            if (RowFirstName.NoGeneration)
                firstName = RowFirstName.Value;
            else
            {
                string format;
                switch (naming)
                {
                    case Gender.NamingList.Male:
                        format = culture.Names.FormatMale;
                        if (string.IsNullOrEmpty(format))
                            firstName = culture.Names.Male.GetRandom();
                        break;
                    case Gender.NamingList.Female:
                        format = culture.Names.FormatFemale;
                        if (string.IsNullOrEmpty(format))
                            firstName = culture.Names.Female.GetRandom();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!string.IsNullOrEmpty(format))
                    firstName = ResolveNameFormat(format, culture.Names);
            }

            if (RowLastName.NoGeneration)
                lastName = RowLastName.Value;
            else
            {
                string format;
                switch (naming)
                {
                    case Gender.NamingList.Male:
                        format = culture.Names.FormatMaleLast;
                        break;
                    case Gender.NamingList.Female:
                        format = culture.Names.FormatFemaleLast;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (string.IsNullOrEmpty(format))
                    lastName = culture.Names.Last.Any() ? culture.Names.Last.GetRandom() : string.Empty;
                else
                    lastName = ResolveNameFormat(format, culture.Names);
            }

            return $"{firstName} {lastName}".Trim();
        }

        private static IReadOnlyDictionary<string, long> CalcMaxAttr(IEnumerable<AttrMod> mods)
        {
            var attrLst = new[] { "MU", "KL", "IN", "CH", "FF", "GE", "KO", "KK" };
            var rndStats = new Dictionary<string, long>(attrLst.Length);
            foreach (var s in attrLst)
                rndStats.Add(s, 0);

            foreach (var mod in mods)
            {
                if(mod.Type == ModType.Or)
                    rndStats[mod.Stats.GetRandom()] += mod.Value;
                else
                {
                    foreach (var s in mod.Stats)
                        rndStats[s] += mod.Value;
                }
            }

            return rndStats;
        }

        public static string ResolveNameFormat(string format, Names names)
        {
            var ret = format;
            if (names.Male != null && names.Male.Any())
                ret = ret.Replace("{male}", names.Male.GetRandom());
            if (names.Female != null && names.Female.Any())
                ret = ret.Replace("{female}", names.Female.GetRandom());
            if (names.Last != null && names.Last.Any())
                ret = ret.Replace("{last}", names.Last.GetRandom());
            if (names.Suffix != null && names.Suffix.Any())
                ret = ret.Replace("{suffix}", names.Suffix.GetRandom());
            return ret;
        }

        public static Stats CalculateStats(Species species, Attributes attr)
        {
            var stats = new Stats
                        {
                                    Lp = species.BaseHp + 2 * attr.Ko,
                                    Sk = species.BaseSk + (int)Math.Floor((attr.Mu + attr.Kl + attr.In) / 6d),
                                    Zk = species.BaseZk + (int)Math.Floor((attr.Ko + attr.Ko + attr.Kk) / 6d),
                                    Aw = attr.Ge / 2,
                                    Ini = (attr.Mu + attr.Ge) / 2,
                                    Gs = species.BaseGs
                        };

            return stats;
        }
    }
}