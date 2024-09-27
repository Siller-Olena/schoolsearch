using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 

namespace Schoolsearch
{//Перечислення осіб - 'Teacher', 'Student', 'Undefined' відповідно класу 
    public enum PersonType { PT_Undefined, PT_Student, PT_Teacher };
    //визначення перерахування
    public enum FilterType { FT_StudentALL, FT_Student, FT_StudentBus, FT_TeacherALL, FT_Teacher, FT_Classroom, FT_Bus };

    public class school
    {
        private List<Student> Students;//визначення списку 'Student'
        private List<Teacher> Teachers;//визначення списку 'Teacher'

        public int Status;

        // Search Maps
        private SortedDictionary<string, List<int>> SearchStudents;         // MAP: Key = Lastname of student --- VALUE = Список of indexes in students
        private SortedDictionary<string, List<int>> SearchTeachers;         // MAP: KEY = Lastname of teacher --- VALUE = Список of indexes in teachers
        private SortedDictionary<int, List<int>> SearchBus;                 // MAP: KEY = Bus No. --- VALUE = List of indexes in students
        private SortedDictionary<int, List<int>> SearchStudentClassroom;    // MAP: KEY = Classroom No. --- VALUE = List of indexes of students
        private SortedDictionary<int, List<int>> SearchTeacherClassroom;    // MAP: KEY = Classroom No. --- VALUE = List of indexes of teachers
        private SortedDictionary<int, List<int>> SearchTeacherGrade;        // MAP: KEY = Grade of student --- VALUE = List of indexes of teachers


        // constructor - Ініціалізація та створення списків і «SearchDictonary»
        public school()
        {
            // Створюємо або налаштуємо списки «Students» та «Teachers»
            Students = new List<Student>();
            Teachers = new List<Teacher>();

            SearchStudents = new SortedDictionary<string, List<int>>();
            SearchTeachers = new SortedDictionary<string, List<int>>();
            SearchBus = new SortedDictionary<int, List<int>>();
            SearchStudentClassroom = new SortedDictionary<int, List<int>>();
            SearchTeacherClassroom = new SortedDictionary<int, List<int>>();
            SearchTeacherGrade = new SortedDictionary<int, List<int>>();
            
            // Викликаємо метод читання файлу «Students.txt». Якщо метод повертає false,
            // глобальна змінна «Status» класу «school» встановлюється в 1,
            // щоб функція main() могла запитати, чи вдалося прочитати файл.
            if (!ReadSchoolData())
            {
                Status = 1; // no file found - inform Main to close application
            }
        }

        //визначає позицію у списку 'Students' на основі переданих даних
        private int GetStudentIndex(string LastName, string aFirstName, int aClassroom, int aBus, int aGrade)
        {
            return Students.FindIndex(x => x.LastName == LastName && x.FirstName == aFirstName && x.Classroom == aClassroom && x.Bus == aBus && x.Grade == aGrade);
        }

        //визначає позицію у списку 'Teacher' на основі переданих даних
        private int GetTeacherIndex(string LastName, string aFirstName, int aClassroom, int aGrade)
        {
            return Teachers.FindIndex(x => x.LastName == LastName && x.FirstName == aFirstName && x.Classroom == aClassroom && x.Grade == aGrade);
        }

        // Прочитайте файл «students.txt». Файл «students.txt» повинен
        // знаходитися в тій самій директорії, що й програма «Schoolsearch.exe»
        public bool ReadSchoolData()
        {
            string fileName = "students.txt";
            if (File.Exists(fileName))                      // Перевіряємо, чи існує файл «students.txt»
            {
                var lines = File.ReadAllLines(fileName);    // Читаємо файл «students.txt» і зберігаємо його у масиві «lines» 
                for (int i = 0; i < lines.Length; i++)      // Проходимося по всіх рядках у циклі і
                {
                    string[] items = lines[i].Split(',');   // Розбиваємо рядок на масив рядків з іменами items
                    if (items.Length == 7)                  // Перевіряємо, чи знайдено рівно 7 елементів.
                    {
                        string aSLastName = items[0];           // Якщо знайдено рівно 7 елементів, окремі елементи слід зберегти 
                        string aSFirstName = items[1];          // у змінних aSLastname, aSFirstname, aGrade, aClassroom, aBus,
                        int aGrade = int.Parse(items[2]);       // aTLastname та aTFirstname. 
                        int aClassroom = int.Parse(items[3]);   
                        int aBus = int.Parse(items[4]);
                        string aTLastName = items[5];
                        string aTFirstName = items[6];

                        // Потім виконуються методи «AddStudent» та «AddTeacher».
                        AddStudent(aSLastName, aSFirstName, aBus, aClassroom, aGrade);
                        AddTeacher(aTLastName, aTFirstName, aClassroom, aGrade);
                    }
                    else
                    {
                        // Якщо знайдено не рівно 7 елементів, виведіть на консоль помилку «Item mismatch line XXX».
                        Console.WriteLine("Item mismatch line {0}", i);
                    }
                }
                return true;    // Значення методу, що повертається, є «true»
            }
            else
                return false;   // Файл не знайдено - повертається значення методу «false»
        }

        // Метод добавлення student
        public void AddStudent(string aSLastName, string aSFirstName, int aBus, int aClassroom, int aGrade)
        {
            try                                                                                        
            {
                // Якщо студента зі значеннями «aSLastname», «aSFirstname», «aClassroom», «aBus»,
                // «aGrade» не існує у списку «Students» (значення, що повертається == -1), то
                if (GetStudentIndex(aSLastName, aSFirstName, aClassroom, aBus, aGrade) == -1)
                {
                    Student student = new Student(aSLastName, aSFirstName, aClassroom, aBus, aGrade);   // створюємо новий екземпляр/об'єкт класу «Student»
                    int idxStudents = Students.Count();                                                 // визначаємо кількість елементів, що вже внесені до списку «Students»
                                                                                                        // («Students.Count» є наступним індексом, оскільки список починається з 0)
                    Students.Add(student);                                                              // Додаємо новостворений екземпляр до списку «Students» в кінці.


                    // Введіть «Student» у полі «SearchStudents».
                    if (SearchStudents.ContainsKey(student.LastName))                                   // Перевіряємо, чи міститься ім'я завантаження в «SearchStudents»
                    {
                        List<int> idxList = SearchStudents[student.LastName];                           // «Student» вже існує, отримуємо збережений Список індексів списку
                        idxList.Add(idxStudents);                                                       // “Students” і додайте індекс нового “Студента” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int> { idxStudents };                              // створіть список для індексів і введіть перший індекс
                        SearchStudents[student.LastName] = idxList;                                     // додати список до Мапи як значення
                    }

                    // Введіть «Student» у полі «SearchBus».
                    if (SearchBus.ContainsKey(aBus))                                                    // Перевіряємо, чи міститься ім'я завантаження в «SearchBus»
                    {
                        List<int> idxList = SearchBus[aBus];                                            // «Bus» вже існує, отримуємо збережений Список індексів списку
                        idxList.Add(idxStudents);                                                       // “Students” і додайте індекс нового “Студента” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int> { idxStudents };                              // створюємо список для індексів і введіть перший індекс
                        SearchBus[aBus] = idxList;                                                      // додати список до Мапи як значення
                    }

                    // Введіть «Student» у полі «SearchClassroom».
                    if (SearchStudentClassroom.ContainsKey(aClassroom))                                 // Перевіряємо, чи міститься ім'я завантаження в «SearchClassroom»
                    {
                        List<int> idxList = SearchStudentClassroom[aClassroom];                         // «Classroom» вже існує, отримуємо збережений Список індексів списку
                        idxList.Add(idxStudents);                                                       // “Students” і додайте індекс нового “Студента” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int>() { idxStudents };                            // створіть список для індексів і введіть перший індекс
                        SearchStudentClassroom[aClassroom] = idxList;                                   // додати список до Мапи як значення
                    }
                }
            }
            catch (Exception)                                                                            
            {
            }
        }

        // Meтод добавлення teacher
        public void AddTeacher(string aTLastName, string aTFirstName, int aClassroom, int aGrade)
        {
            try                                                                                         
            {
                // Якщо студента зі значеннями «aTLastname», «aTFirstname», «aClassroom»,
                // «aGrade» не існує у списку «Teachers» (значення, що повертається == -1), то
                if (GetTeacherIndex(aTLastName, aTFirstName, aClassroom, aGrade) == -1)
                {
                    Teacher teacher = new Teacher(aTLastName, aTFirstName, aClassroom, aGrade);         // створюємо новий екземпляр/об'єкт класу «Teacher»
                    int idxTeachers = Teachers.Count();                                                 // визначаємо кількість елементів, що вже внесені до списку «Teachers»
                                                                                                        // («Teachers.Count» є наступним індексом, оскільки список починається з 0)
                    Teachers.Add(teacher);                                                              // Додаємо новостворений екземпляр до списку «Students» в кінці.

                    // Введіть «Teacher» у полі «SearchTeachers».
                    if (SearchTeachers.ContainsKey(teacher.LastName))                                   // Перевіряємо, чи міститься ім'я завантаження в «SearchTeachers»
                    {
                        List<int> idxList = SearchTeachers[teacher.LastName];                           // «Teacher» вже існує, отримайте збережений Список індексів списку
                        idxList.Add(idxTeachers);                                                       // “Teachers” і додайте індекс нового “Student” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int> { idxTeachers };                              // створюємо список для індексів і введіть перший індекс
                        SearchTeachers[teacher.LastName] = idxList;                                     // додаємо список до Мапи як значення
                    }

                    // Введіть «Student» у полі «SearchClassroom».
                    if (SearchTeacherClassroom.ContainsKey(aClassroom))                                 // Перевіряємо, чи міститься ім'я завантаження в «SearchClassroom» 
                    {
                        List<int> idxList = SearchTeacherClassroom[aClassroom];                         // «Classroom» вже існує, отримайте збережений Список індексів списку
                        idxList.Add(idxTeachers);                                                       // “Students” і додаємо індекс нового “Студента” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int>() { idxTeachers };                            // створюємо список для індексів і введіть перший індекс
                        SearchTeacherClassroom[aClassroom] = idxList;                                   // додаємо список до Мапи як значення 
                    }

                    // Введіть «Student» у полі «SearchTeacherGrade».
                    if (SearchTeacherGrade.ContainsKey(aGrade))                                         // Перевіряємо, чи міститься ім'я завантаження в «SearchTeacherGrade» 
                    {
                        List<int> idxList = SearchTeacherGrade[aGrade];                                 // «Grade» вже існує, отримуємо збережений Список індексів списку
                        idxList.Add(idxTeachers);                                                       // “Teacher” і додаємо індекс нового “Студента” до списку
                    }
                    else
                    {
                        List<int> idxList = new List<int>() { idxTeachers };                            // створюємо список для індексів і введемо перший індекс
                        SearchTeacherGrade[aGrade] = idxList;                                           // додаємо список до Мапи як значення
                    }
                }
            }
            catch (Exception)                                                                           
            {
            }
        }

        // Фунуція фільтра для вибору іншого фільтра (teacher, student, bus, classroom)
        // ця функція використовує карти, які будуються під час завантаження та закінчуються додаванням teacher и/или student
        public void GetFilteredData(ref string data, FilterType aFilterType, ref int RecordCount, string aFilterValue = "", int aIntFilterValue = -1)
        {
            //проверка если LastName має значення. Якщо немає значення, то встановлює фільт для усіх 'Studens'
            if (aFilterType == FilterType.FT_Student && aFilterValue == "")
                aFilterType = FilterType.FT_StudentALL;
            //проверка если LastName має значення. Якщо немає значення, то встановлює фільт для усіх 'Teaches'
            if (aFilterType == FilterType.FT_Teacher && aFilterValue == "")
                aFilterType = FilterType.FT_TeacherALL;
            //проверка если Bus має значення. Якщо немає значення, то не шукає
            if (aIntFilterValue == -1 && aFilterType == FilterType.FT_Bus)
            {
                data = "Incorrect bus No.! Search aborted!";
                RecordCount = 0;
                return;
            }
            //проверка если Classroom має значення. Якщо немає значення, то не шукає
            if (aIntFilterValue == -1 && aFilterType == FilterType.FT_Classroom)
            {
                data = "Incorrect classroom! Search aborted!";
                RecordCount = 0;
                return;
            }

            switch (aFilterType)
            {
                //пошук усіх Students
                case FilterType.FT_StudentALL:
                    {
                        //дивимось список Students. Якщо немає, то пише текст що не знайшов
                        if (Students.Count == 0)
                        {
                            data = "No students found!";
                            RecordCount = 0;

                            return;
                        }
                        else
                        {
                            for (int i = 0; i < Students.Count; i++)                // Цикл для перебору списку «Students»
                            {
                                Student student = Students[i];                      // Копіюємо екземпляр «Student» у змінну «student»
                                if (i == 0)                                         // Якщо це перший екземпляр списку, то збережіть заголовок таблиці в «data» 
                                    data = student.GetFormatedHeader();             // заголовок таблиці в «data»
                                data = data + "\r\n" + student.GetFormatedData();   // а потім додаємо відформатовані дані про студента після переходу на новий рядок у «data»
                            }
                            RecordCount = Students.Count;                           // Зберегаємо кількість індексних списків «Students» у списку у «Recordcount».

                        }
                    }
                    break;
                case FilterType.FT_Student:
                    {
                        if (!SearchStudents.ContainsKey(aFilterValue))              // Перевіряємо, чи значення фільтра, введене для «Прізвище», не міститься в MAP «SearchStudents»
                        {
                            data = string.Format("No student with the name {0} found!", aFilterValue); // Вивід для консолі «No student with the name XXXXXXX found!»
                            RecordCount = 0;                                        // Встановлюємо «RecordCount» на 0

                            return;                                                 // Метод виходу
                        }
                        else
                        {
                            List<int> idxList = SearchStudents[aFilterValue];       // Копіюємо список з індексами студентів з карти «SearchStudents» у локальну змінну
                            for (int i = 0; i < idxList.Count; i++)                 // Ітерація над списком індексів
                            {
                                int idx = idxList[i];                               // Зберігаємо індекс зі списку «Students» у локальній змінній «idx»
                                Student student = Students[idx];                    // Зберігаємо екземпляр студента в локальній змінній за індексом зі списку «Students»
                                if (i == 0)                                         // Перевіртяємо, чи це перший запис, якщо так, створіть заголовок таблиці
                                    data = student.GetFormatedHeader();            

                                data = data + "\r\n" + student.GetFormatedData();   // Додаємо дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка
                            }
                            RecordCount = idxList.Count;                            // Зберегти кількість індексних списків «Students» у списку у «Recordcount».

                        }
                    }
                    break;
                case FilterType.FT_StudentBus:
                    {
                        if (!SearchStudents.ContainsKey(aFilterValue))              // Перевіряємо, чи значення фільтра, введене для «LastName», не міститься в MAP «SearchStudents»
                        {
                            data = string.Format("No student with the name {0} found!", aFilterValue); // Вивід для консолі «No student with the name XXXXXXX found!»
                            RecordCount = 0;                                        // Встановлюємо «RecordCount» на 0
                            return;                                                 // Метод виходу
                        }
                        else
                        {
                            int count = 0;
                            List<int> idxList = SearchStudents[aFilterValue];       // Копіюємо список з індексами студентів з карти «SearchStudents» у локальну змінну
                            for (int i = 0; i < idxList.Count; i++)                 // Ітерація над списком індексів
                            {
                                
                                int idx = idxList[i];                               // Зберігаємо індекс зі списку індексів у локальній змінній «idx».
                                Student student = Students[idx];                    // Зберігаємо екземпляр студента в локальній змінній за індексом зі списку «Students»
                                if (i == 0)                                         // Перевіряємо, чи це перший запис, якщо так, створіть заголовок таблиці
                                    data = student.GetFormatedHeader();

                                if (student.Bus == aIntFilterValue)                 // Перевіряємо, чи «Bus» збігається зі значенням фільтра, введеним для «Student.Bus»
                                {
                                    data = data + "\r\n" + student.GetFormatedData();// Додайте дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка
                                    count++;                                        // збільшити число на 1
                                }
                            }
                            RecordCount = count;                                    // Зберігаємо кількість елементів списку в «RecordCount»
                        }
                    }
                    break;
                case FilterType.FT_TeacherALL:
                    {
                        // шукає у списку студентів. Якщо ні, то пише текст, що його не знайдено і встановлює число для «RecordCount в 0
                        if (Teachers.Count == 0)
                        {
                            data = "No teachers found!";
                            RecordCount = 0;

                            return;                                                 // Метод виходу
                        }
                        else
                        {
                            for (int i = 0; i < Teachers.Count; i++)                // Цикл для перебору списку «Teachers»
                            {
                                Teacher teacher = Teachers[i];                      // Копіюємо екземпляр «Teacher» у змінну «teacher»
                                if (i == 0)                                         // Якщо це перший екземпляр списку, то збережіть заголовок таблиці в «data» 
                                    data = teacher.GetFormatedHeader();             // заголовок таблиці в «data»
                                
                                data = data + "\r\n" + teacher.GetFormatedData();   // а потім додайте відформатовані дані про студента після переходу на новий рядок у «data»
                            }
                            RecordCount = Teachers.Count;                           // Зберегти кількість індексних списків «Teachers» у списку у «Recordcount».

                        }
                    }
                    break;
                case FilterType.FT_Teacher:
                    {
                        // шукає у списку студентів. Якщо ні, то пише текст, що його не знайдено і встановлює число для «RecordCount в 0
                        if (!SearchTeachers.ContainsKey(aFilterValue))
                        {
                            data = string.Format("No teacher with the name {0} found!", aFilterValue);
                            RecordCount = 0;

                            return;                                                 // Метод виходу
                        }
                        else
                        {
                            List<int> idxList = SearchTeachers[aFilterValue];
                            for (int i = 0; i < idxList.Count; i++)
                            {
                                int idx = idxList[i];
                                Teacher teacher = Teachers[idx];                    // Копіюємо екземпляр «Teacher» у змінну «teacher»
                                if (i == 0)                                         // Якщо це перший екземпляр списку, то збережіть заголовок таблиці в «data» 
                                    data = teacher.GetFormatedHeader();             // заголовок таблиці в «data»

                                data = data + "\r\n" + teacher.GetFormatedData();   // а потім додайте відформатовані дані про студента після переходу на новий рядок у «data»
                            }
                            RecordCount = idxList.Count;                            // Зберегти кількість індексних списків «Teachers» у списку у «Recordcount».


                        }
                    }
                    break;

                case FilterType.FT_Classroom:
                    {
                        RecordCount = 0;
                        // Перевірити, чи значення фільтра, введене для «Прізвище»,
                        // не міститься в MAP «SearchStudentClassroom» і не міститься в MAP «SearchTeacherClassroom»
                        if (!SearchStudentClassroom.ContainsKey(aIntFilterValue) && 
                            !SearchTeacherClassroom.ContainsKey(aIntFilterValue))
                        {
                            data = "No person for this classroom found!";           // Вивід для консолі «No person for this classroom found!»
                            return;                                                 // Метод виходу
                        }

                        if (SearchStudentClassroom.ContainsKey(aIntFilterValue))    // Перевірити, чи значення фільтра, введене для «Lastname», міститься в карті «SearchStudentClassroom»

                        {
                            List<int> idxList = SearchStudentClassroom[aIntFilterValue];// Копіюємо список з індексами студентів з карти «SearchStudentClassroom» у локальну змінну
                            
                            for (int i = 0; i < idxList.Count; i++)                 // Ітерація над списком індексів
                            {
                                int idx = idxList[i];                               // Зберегти індекс зі списку індексів у локальній змінній «idx».
                                Student student = Students[idx];                    // Збережіть екземпляр студента в локальній змінній за індексом зі списку «Students»
                                if (i == 0)                                         // Перевірте, чи це перший запис, якщо так, створіть заголовок таблиці
                                    data = student.GetFormatedHeader();

                                data = data + "\r\n" + student.GetFormatedData();   // Додайте дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка
                            }
                            RecordCount += idxList.Count;                           // Збережіть кількість елементів списку в «RecordCount»
                        }

                        if (SearchTeacherClassroom.ContainsKey(aIntFilterValue))    // Перевірити, чи значення фільтра, введене для «Lastname», міститься в карті «SearchTeacherClassroom»
                        {
                            List<int> idxList = SearchTeacherClassroom[aIntFilterValue];// Копіюємо список з індексами студентів з карти «SearchTeacherClassroom» у локальну змінну
                            for (int i = 0; i < idxList.Count; i++)                 // Ітерація над списком індексів
                            {
                                int idx = idxList[i];                               // Зберегти індекс зі списку індексів у локальній змінній «idx».
                                Teacher teacher = Teachers[idx];                    // Збережіть екземпляр студента в локальній змінній за індексом зі списку «Teachers»

                                data = data + "\r\n" + teacher.GetFormatedData();   // Додайте дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка
                            }
                            RecordCount += idxList.Count;                           // Додайте кількість елементів списку до «RecordCount» і збережіть у полі «RecordCount».
                        }
                    }
                    break;
                case FilterType.FT_Bus:
                    {
                        if (!SearchBus.ContainsKey(aIntFilterValue))                // Перевірити, чи значення фільтра, введене для «Bus», не міститься в MAP «SearchBus»
                        {
                            data = string.Format("No students for the bus {0} found!", aIntFilterValue.ToString());
                            RecordCount = 0;                                        // Встановіть «RecordCount» на 0

                            return;                                                 // Метод виходу
                        }
                        else
                        {
                            List<int> idxList = SearchBus[aIntFilterValue];         // Копіюємо список з індексами студентів з карти «SearchBus» у локальну змінну
                            for (int i = 0; i < idxList.Count; i++)                 // Ітерація над списком індексів
                            {
                                int idx = idxList[i];                               // Зберегти індекс зі списку індексів у локальній змінній «idx».
                                Student student = Students[idx];                    // Збережіть екземпляр студента в локальній змінній за індексом зі списку «Students»
                                if (i == 0)
                                    data = student.GetFormatedHeader();             // Додайте дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка

                                data = data + "\r\n" + student.GetFormatedData();   // Додайте дані студента до змінної з допомогою методу «GetFormatedData()» з переведенням рядка
                            }
                            RecordCount = idxList.Count;                            // Зберегти кількість індексних списків у списку у «Recordcount».
                        }
                    }
                    break;
                default:
                    {
                        // Якщо жодна з умов не виконується Створіть список «Студентів» та «Викладачів»
                        if (Students.Count == 0 && Teachers.Count == 0)
                        {
                            data = "No persons found!";
                            return;                                                     // Метод виходу
                        }

                        for (int i = 0; i < Students.Count; i++)                        // Цикл для перебору списку «Students»
                        {
                            Student student = Students[i];                              // Копіюємо екземпляр «Student» у змінну «student»
                            if (i == 0)                                                 // Якщо це перший екземпляр списку, то збережіть заголовок таблиці в «data» 
                                data = student.GetFormatedHeader();                     // заголовок таблиці в «data»
                            data = data + "\r\n" + student.GetFormatedData();           // а потім додайте відформатовані дані про студента після переходу на новий рядок у «data»
                        }
                        for (int i = 0; i < Teachers.Count; i++)                        // Цикл для перебору списку «Teachers»
                        {
                            Teacher teacher = Teachers[i];                              // Копіюємо екземпляр «Teacher» у змінну «teacher»
                            data = data + "\r\n" + teacher.GetFormatedData();           // а потім додайте відформатовані дані про студента після переходу на новий рядок у «data»
                        }
                        RecordCount = Students.Count + Teachers.Count;                  // Додайте кількість елементів списку «Студенти» до кількості елементів
                                                                                        // списку «Викладачі» до «Кількість записів» і збережіть у полі
                                                                                        // «Кількість записів».
                    }
                    break;
            }
        }
    }
}
