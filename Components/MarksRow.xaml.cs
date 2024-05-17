using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StudentAttendanceMarks.Database;

namespace StudentAttendanceMarks.Components
{


    /// <summary>
    /// Interaction logic for MarksRow.xaml
    /// </summary>
    public partial class MarksRow
    {
        public MarksRow()
        {
            InitializeComponent();
        }
        public MarksRow(int index, Marks marks)
        {
            InitializeComponent();
            DataContext = marks;
            Student? student;
            using (var db = new StudentAttendanceMarksDB())
                student = db.GetStudentById(marks.StudentId);
            
            if (student is null) return;
            
            


            IndexLabel.Content = index;

            Photo.Source = ReadBitmapImage(student.Photo);
            FullNameLabel.Content = student.FullName;
            LastOnlineLabel.Content = student.LastOnline.Date;
            
            var defaultComboBoxItem = new ComboBoxItem
            {
                Content = " - ",
                Tag = "defaultComboBox",
                Visibility = Visibility.Collapsed
            };

            GradesComboBox.Items.Add(defaultComboBoxItem);
            GradesComboBox.SelectedItem = defaultComboBoxItem;


            switch (marks.AttendanceMark)
            {
                case AttendanceMark.PRESENT:
                    PresentRadioButton.IsChecked = true;
                    break;
                case AttendanceMark.LATE:
                    LateRadioButton.IsChecked = true;
                    break;
                case AttendanceMark.ABSENT:
                    AbsentRadioButton.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (marks.Grade == null) return;
            GradesComboBox.SelectedItem = marks.Grade;
            GradesComboBox.Text = marks.Grade.ToString();

        }
        private BitmapImage ReadBitmapImage(byte[] imageBytes)
        {


            // Преобразование byte[] в BitmapImage
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();

            return bitmapImage;

        }

        private void RadioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Marks? marks = DataContext as Marks;
            if(marks is null) return;

            var selectedMark = sender as RadioButton;
            if (selectedMark is null) return;


            using var db = new StudentAttendanceMarksDB();
            var dbSelectedMark = db.GetAllMarks().FirstOrDefault(m => m.Id == marks.Id);
            if (dbSelectedMark is null) return;
            
            dbSelectedMark.AttendanceMark = Enum.Parse<AttendanceMark>(selectedMark.Tag.ToString());
            db.SaveChanges();

        }

        private void GradesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Marks? marks = DataContext as Marks;
            if(marks is null) return;
            

            ComboBoxItem? selectedGrade = GradesComboBox.SelectedItem as ComboBoxItem;
            
            
            if (selectedGrade is null ||
                ReferenceEquals(selectedGrade.Tag, "defaultComboBox")) return;


            using var db = new StudentAttendanceMarksDB();
            var dbSelectedMark = db.GetAllMarks().FirstOrDefault(m => m.Id == marks.Id);
            if (dbSelectedMark is null) return;
            
            dbSelectedMark.Grade = int.Parse(selectedGrade.Content.ToString() ?? throw new InvalidOperationException());
            db.SaveChanges();
        }
    }
}
