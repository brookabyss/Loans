namespace Loans.Models
{
    public class ModelBundle
    {
        public LendersView LendersModel { get; set; }
        public BorrowersView BorrowersModel { get; set; }
        public ModelBundle(){
            LendersModel= new LendersView();
            BorrowersModel = new BorrowersView();
        }
    }
}