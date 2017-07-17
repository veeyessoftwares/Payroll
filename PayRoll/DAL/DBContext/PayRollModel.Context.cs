namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class PayRollEntities : DbContext
    {
        public PayRollEntities()
            : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<ATTENDANCE_IMPORT> ATTENDANCE_IMPORT { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<UNIT> UNITs { get; set; }
        public virtual DbSet<WAGESTYPE> WAGESTYPEs { get; set; }
    }
}
