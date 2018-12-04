//-----------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="None">
//     Copyright (c) Allow to distribute this code and utilize this code for personal or commercial purpose.
// </copyright>
// <author>Asma Khalid</author>
//-----------------------------------------------------------------------

namespace SistemaCadastro.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Login view model class.
    /// </summary>
    public class Login
    {
        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Column("LoginID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets to username address.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets to password address.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        #endregion
    }
}
