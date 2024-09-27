
namespace Schoolsearch
{
    public class Student : Person //клас 'Student' є похідним від класу 'Person'
    {
        private int classroom;
        private int bus;
        private int grade;

        public int Classroom { get => classroom; } //властивості до сlassroom, тільки для прочитання
        public int Bus { get => bus; }
        public int Grade { get => grade; }

        public Student(string aLastName, string aFirstName, int aClassroom, int aBus, int aGrade) : base(aLastName, aFirstName, PersonType.PT_Student)
        //конструктор класу
        {
            classroom = aClassroom;//задаються приватні змінні
            bus = aBus;
            grade = aGrade;
        }

        public override string GetFormatedHeader()//абстрактні методи класу 'Person' реалізовано та переписано для похідного класу 'Student'       
        {//метод класу 'Student' який повертає рядок заголовку вихідної таблиці у вигляді рядка
            string data = "╔════════════╦═════════════════════════╦═════════════════════════╦════════════╦════════╦════════╗\r\n";
            data += string.Format("║{0,-12}║{1,-25}║{2,-25}║{3,-12:0}║{4,-8:0}║{5,-8}║\r\n", "  Type", " LastName", " Firstname", " Classroom", "Bus", " Grade");
            data += "╠════════════╬═════════════════════════╬═════════════════════════╬════════════╬════════╬════════╣";
            return data;
        }

        public override string GetFormatedData()
        {// метод класу 'Student'що виводить данні про Teacher у вигляді рядка таблиці
            return string.Format("║{0,-12}║{1,-25}║{2,-25}║{3,-12:0}║{4,-8:0}║{5,-8}║", " Student", " " + LastName, " " + FirstName, "    " + Classroom, " " + Bus, "   " + Grade);
        }
    }
}
