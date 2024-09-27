using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoolsearch
{
    //базовий клас студенів і учителів
    public abstract class Person
    {
        private string lastName;// змінна для фамілії
        private string firstName;
        private PersonType type; //змінна для перерахування (PT_Undefined, PT_Student, PT_Teacher) 

        public string LastName { get => lastName; }//властивості до lastName, тільки для прочитання
        public string FirstName { get => firstName; }

        public Person(string aLastName, string aFirstName, PersonType aType)//конструктор класу
        {
            lastName = aLastName;//задаються приватні змінні 
            firstName = aFirstName;
            type = aType;
        }
        public abstract string GetFormatedHeader();
        public abstract string GetFormatedData();//абстрактні методи, які реалізуютяся в класах
                                                 //'Student'і'Teacher'
    }
}
