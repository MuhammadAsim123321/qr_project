using Humanizer;
using Identity_Login.Models;
using Identity_Login.Models.dbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.Security.Claims;
using static QuestPDF.Helpers.Colors;

namespace Identity_Login.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<JobProcess> JobProcesses { get; set; }
        public DbSet<JobProcessStage> JobProcessStages { get; set; }
        public DbSet<ProcessStep> ProcessSteps { get; set; }
        public DbSet<RouterJob> RouterJobs { get; set; }
        public DbSet<StaffStationMapping> StaffStationMappings { get; set; }
        public DbSet<UploadImage> UploadImage { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Classification> classifications { get; set; }
        public DbSet<RunType> RunTypes { get; set; }

        public DbSet<ASF> ASFs { get; set; }
        public DbSet<Materail> Materails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Materail>().HasData(
                new Materail { MaterailId = 1, Name = "5052 AL" },
                new Materail { MaterailId = 2, Name = "6061 AL" },
                new Materail { MaterailId = 3, Name = "7000 AL" },
                new Materail { MaterailId = 4, Name = "MIC-6 AL" },
                new Materail { MaterailId = 5, Name = "Stainless steel" },
                new Materail { MaterailId = 6, Name = "Steel" },
                new Materail { MaterailId = 7, Name = "Material 2024" }


            );

            builder.Entity<ASF>().HasData(
                new ASF { ASFId = 1, Name = "8 ASF" },
                new ASF { ASFId = 2, Name = "10 ASF" },
                new ASF { ASFId = 3, Name = "12 ASF" },
                new ASF { ASFId = 4, Name = "16 ASF" },
                new ASF { ASFId = 5, Name = "24 ASF" }
            );

            builder.Entity<Classification>().HasData(
                new Classification { ClassificationId = 1, Name = "Type I, Class 1 (CLEAR)", Minutes = 12 },
                new Classification { ClassificationId = 2, Name = "Type II, Class 1 (CLEAR)", Minutes = 18 },
                new Classification { ClassificationId = 3, Name = "Type II, Class 2 (BLACK)", Minutes = 45 },
                new Classification { ClassificationId = 4, Name = "Type II, Class 2 (BLUE-A)", Minutes = 38 },
                new Classification { ClassificationId = 5, Name = "Type II, Class 2 (BORDEAUX RED)", Minutes = 38 },
                new Classification { ClassificationId = 6, Name = "Type II, Class 2 (BROWN-GL)", Minutes = 30 },
                new Classification { ClassificationId = 7, Name = "Type II, Class 2 (CAMO BROWN)", Minutes = 18 },
                new Classification { ClassificationId = 8, Name = "Type II, Class 2 (DARK BLUE)", Minutes = 45 },
                new Classification { ClassificationId = 9, Name = "Type II, Class 2 (GOLD S)", Minutes = 30 },
                new Classification { ClassificationId = 10, Name = "Type II, Class 2 (GREEN AEN)", Minutes = 45 },
                new Classification { ClassificationId = 11, Name = "Type II, Class 2 (GREY)", Minutes = 12 },
                new Classification { ClassificationId = 12, Name = "Type II, Class 2 (LANTZ MEDICAL BLUE)", Minutes = 25 },
                new Classification { ClassificationId = 13, Name = "Type II, Class 2 (NEON PINK)", Minutes = 30 },
                new Classification { ClassificationId = 14, Name = "Type II, Class 2 (OLIVE DRAB)", Minutes = 35 },
                new Classification { ClassificationId = 15, Name = "Type II, Class 2 (ORANGE 2B)", Minutes = 38 },
                new Classification { ClassificationId = 16, Name = "Type II, Class 2 (TEAL)", Minutes = 38 },
                new Classification { ClassificationId = 17, Name = "Type II, Class 2 (VIOLET 3D)", Minutes = 45 },
                new Classification { ClassificationId = 18, Name = "Type II, Class 2 (YELLOW 4A)", Minutes = 15 },
                new Classification { ClassificationId = 19, Name = "Type III, Class 1 (CLEAR)", Minutes = 105 },
                new Classification { ClassificationId = 20, Name = "Type III, Class 1 (CLEAR) W/ PTFE TEFLON", Minutes = 105 },
                new Classification { ClassificationId = 21, Name = "Type III, Class 1 (CLEAR)(2 MIL)", Minutes = 140 },
                new Classification { ClassificationId = 22, Name = "Type III, Class 2 (BLACK)", Minutes = 105 },
                new Classification { ClassificationId = 23, Name = "ASTM A 967, NITRIC", Minutes = 30 },
                new Classification { ClassificationId = 24, Name = "ASTM A 967, CITRIC", Minutes = 30 },
                new Classification { ClassificationId = 25, Name = "Black Oxide", Minutes = 0 },
                new Classification { ClassificationId = 26, Name = "Stainless Steel", Minutes = 0 },
                new Classification { ClassificationId = 27, Name = "Steel", Minutes = 0 },
                new Classification { ClassificationId = 28, Name = "Type I Gold Class 1A Chem Film", Minutes = 15 },
                new Classification { ClassificationId = 29, Name = "Type I Gold Class 3 Chem Film", Minutes = 10 },
                new Classification { ClassificationId = 30, Name = "Type II Clear Class 1A Chem Film", Minutes = 15 },
                new Classification { ClassificationId = 31, Name = "Type II Clear Class 3 Chem Film", Minutes = 10 }

            );

            builder.Entity<RunType>().HasData(
                new RunType { RunTypeId = 1, Name = "ETCH 3 MIN" },
                new RunType { RunTypeId = 2, Name = "ETCH 5 MIN" },
                new RunType { RunTypeId = 3, Name = "EXPED" },
                new RunType { RunTypeId = 4, Name = "NO ETCH" },
                new RunType { RunTypeId = 5, Name = "NO ETCH - CAST MTRL" },
                new RunType { RunTypeId = 6, Name = "NO ETCH/5 MIN DESMUT" },
                new RunType { RunTypeId = 7, Name = "NO SEAL" },
                new RunType { RunTypeId = 8, Name = "NO SEAL/PTFE TEFLON" },
                new RunType { RunTypeId = 9, Name = "PTFE TEFLON" },
                new RunType { RunTypeId = 10, Name = "RUN ON LG AL RACK" },
                new RunType { RunTypeId = 11, Name = "RUN ON MD AL RACK" },
                new RunType { RunTypeId = 12, Name = "RUN ON ROUND RACK" },
                new RunType { RunTypeId = 13, Name = "RUN ON SQ RACK" },
                new RunType { RunTypeId = 14, Name = "STRIP 10 MIN/ETCH 5 MIN" },
                new RunType { RunTypeId = 15, Name = "STRP 6 MIN/ETCH 2 MIN" },
                new RunType { RunTypeId = 16, Name = "STRIP ONLY" },
                new RunType { RunTypeId = 17, Name = "STRIP/ RE-ANODIZE" },
                new RunType { RunTypeId = 18, Name = "2 Min Etch" },
                new RunType { RunTypeId = 19, Name = "Dichromate Seal" },
                new RunType { RunTypeId = 20, Name = "Hot Water Seal" }
            );

            // ===== SEED PROCESSES =====
            builder.Entity<JobProcess>().HasData(
                new JobProcess { ProcessId = 1, Name = "Anodizing Process", Description = "Handles anodizing jobs" },
                new JobProcess { ProcessId = 2, Name = "Passivation Process (Method 1)", Description = "Handles passivation jobs (method 1)" },
                new JobProcess { ProcessId = 3, Name = "Passivation Process (Method 2)", Description = "Handles passivation jobs (method 2)" },
                new JobProcess { ProcessId = 4, Name = "Black Oxide Process", Description = "Handles black oxide jobs" },
                new JobProcess { ProcessId = 5, Name = "Chemical Conversion", Description = "Handles chemical conversion jobs" }

            );

            // ===== SEED PROCESS STEPS =====
            builder.Entity<ProcessStep>().HasData(
                // Anodizing Steps (ProcessId = 1)
                new ProcessStep { ProcessStepId = 1, ProcessId = 1, StepOrder = 1, StepName = "Create router/work order" },
                new ProcessStep { ProcessStepId = 2, ProcessId = 1, StepOrder = 2, StepName = "Ready for racking" },
                new ProcessStep { ProcessStepId = 3, ProcessId = 1, StepOrder = 3, StepName = "Rack parts" },
                new ProcessStep { ProcessStepId = 4, ProcessId = 1, StepOrder = 5, StepName = "Masking (some parts may need this process)" },
                new ProcessStep { ProcessStepId = 5, ProcessId = 1, StepOrder = 6, StepName = "Anodize Process or Chemical conversion" },
                new ProcessStep { ProcessStepId = 6, ProcessId = 1, StepOrder = 7, StepName = "Pack up parts" },
                new ProcessStep { ProcessStepId = 7, ProcessId = 1, StepOrder = 8, StepName = "Ready for shipping" },
                new ProcessStep { ProcessStepId = 8, ProcessId = 1, StepOrder = 9, StepName = "Shipped" },

                // Passivation Process (Method 1) (ProcessId = 2)
                new ProcessStep { ProcessStepId = 9, ProcessId = 2, StepOrder = 1, StepName = "Create router/work order" },
                new ProcessStep { ProcessStepId = 10, ProcessId = 2, StepOrder = 2, StepName = "Ready for racking" },
                new ProcessStep { ProcessStepId = 11, ProcessId = 2, StepOrder = 3, StepName = "Rack parts" },
                new ProcessStep { ProcessStepId = 12, ProcessId = 2, StepOrder = 5, StepName = "Passivation process" },
                new ProcessStep { ProcessStepId = 13, ProcessId = 2, StepOrder = 6, StepName = "Dry parts" },
                new ProcessStep { ProcessStepId = 14, ProcessId = 2, StepOrder = 7, StepName = "Pack up parts" },
                new ProcessStep { ProcessStepId = 15, ProcessId = 2, StepOrder = 8, StepName = "Ready for shipping" },
                new ProcessStep { ProcessStepId = 16, ProcessId = 2, StepOrder = 9, StepName = "Shipped" },

                // Passivation Process (Method 2) (ProcessId = 3)
                new ProcessStep { ProcessStepId = 17, ProcessId = 3, StepOrder = 1, StepName = "Create router/work order" },
                new ProcessStep { ProcessStepId = 18, ProcessId = 3, StepOrder = 2, StepName = "Ready for racking" },
                new ProcessStep { ProcessStepId = 19, ProcessId = 3, StepOrder = 3, StepName = "Rack parts" },
                new ProcessStep { ProcessStepId = 20, ProcessId = 3, StepOrder = 5, StepName = "Passivation process" },
                new ProcessStep { ProcessStepId = 21, ProcessId = 3, StepOrder = 6, StepName = "Dry parts" },
                new ProcessStep { ProcessStepId = 22, ProcessId = 3, StepOrder = 7, StepName = "Pack up parts" },
                new ProcessStep { ProcessStepId = 23, ProcessId = 3, StepOrder = 8, StepName = "Ready for shipping" },
                new ProcessStep { ProcessStepId = 24, ProcessId = 3, StepOrder = 9, StepName = "Shipped" },

                // Black Oxide Process (ProcessId = 4)
                new ProcessStep { ProcessStepId = 25, ProcessId = 4, StepOrder = 1, StepName = "Create router/work order" },
                new ProcessStep { ProcessStepId = 26, ProcessId = 4, StepOrder = 2, StepName = "Ready for racking" },
                new ProcessStep { ProcessStepId = 27, ProcessId = 4, StepOrder = 4, StepName = "Black oxide parts" },
                new ProcessStep { ProcessStepId = 28, ProcessId = 4, StepOrder = 5, StepName = "Pack up parts" },
                new ProcessStep { ProcessStepId = 29, ProcessId = 4, StepOrder = 6, StepName = "Ready for Shipping" },
                new ProcessStep { ProcessStepId = 30, ProcessId = 4, StepOrder = 7, StepName = "Shipped" },

                // Chemical Conversion Process (ProcessId = 5)
                new ProcessStep { ProcessStepId = 38, ProcessId = 5, StepOrder = 1, StepName = "Create router/work order" },
                new ProcessStep { ProcessStepId = 31, ProcessId = 5, StepOrder = 2, StepName = "Class 1A Thick Coating" },
                new ProcessStep { ProcessStepId = 32, ProcessId = 5, StepOrder = 3, StepName = "Class 3 Thin Coating" },
                new ProcessStep { ProcessStepId = 37, ProcessId = 5, StepOrder = 4, StepName = "In-Process" },



                 new ProcessStep { ProcessStepId = 33, ProcessId = 1, StepOrder = 4, StepName = "In-Process" },
                 new ProcessStep { ProcessStepId = 34, ProcessId = 2, StepOrder = 4, StepName = "In-Process" },
                 new ProcessStep { ProcessStepId = 35, ProcessId = 3, StepOrder = 4, StepName = "In-Process" },
                 new ProcessStep { ProcessStepId = 36, ProcessId = 4, StepOrder = 3, StepName = "In-Process" }
                 //new ProcessStep { ProcessStepId = 37, ProcessId = 5, StepOrder = 4, StepName = "In-Process" }

                );

        }
    }
}
