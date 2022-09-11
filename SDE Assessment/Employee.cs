namespace SDE_Assessment
{
    public class Employee
    {
        private string id;
        private long salary = 0;
        private string manager = "";

        /// <summary>
        /// sets and gets employee's id
        /// </summary>
        public string Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// sets and get the id of the personell who this employee reports to
        /// </summary>
        public string Manager
        {
            get => manager;
            set => manager = value;
        }

        /// <summary>
        /// sets and gets employees Salary
        /// </summary>
        public long Salary
        {
            get => salary;
            set => salary = value;
        }

        public override bool Equals(object obj)
        {
            Employee emp1 = (Employee)obj;
            return (emp1.Id.ToUpper().Equals(Id.ToUpper()));
        }
    }
}
