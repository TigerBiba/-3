//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Практическая_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Staff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            this.Departament = new HashSet<Departament>();
            this.Recordind = new HashSet<Recordind>();
            this.Reports = new HashSet<Reports>();
        }
    
        public int ID_staff { get; set; }

        [Required(ErrorMessage = "Специальность должна быть указана")]
        [Range(1, 4)]
        public int speciality { get; set; }

        [Required(ErrorMessage = "Стаж работы должын быть указан")]
        [StringLength(2, MinimumLength = 1)]
        [Range(1, 99)]
        public string work_experience { get; set; }
        public int ID_departament { get; set; }
        public int ID_login { get; set; }

        [Required(ErrorMessage = "Имя должно быть указано")]
        [StringLength(200, MinimumLength = 1)]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Фамилия должна быть указана")]
        [StringLength(200, MinimumLength = 1)]
        public string secondname { get; set; }
        public string photo { get; set; }

        [Required(ErrorMessage = "Email должен быть указан")]
        [StringLength(200, MinimumLength = 1)]
        public string email { get; set; }

        [Required(ErrorMessage = "Серия пасспорта должна быть указан")]
        [Range(1000, 9999)]
        public long series { get; set; }

        [Required(ErrorMessage = "Номер пасспорта должен быть указан")]
        [Range(100000, 999999)]
        public long nomber { get; set; }

        [Required(ErrorMessage = "Кем выдан должен быть указан")]
        [StringLength(200, MinimumLength = 1)]
        public string issued { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Departament> Departament { get; set; }
        public virtual Departament Departament1 { get; set; }
        public virtual Login Login { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recordind> Recordind { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reports> Reports { get; set; }
        public virtual Speciality Speciality1 { get; set; }
    }
}
