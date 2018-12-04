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

    /// <summary>
    /// Login view model class.
    /// </summary>
    public class MessageViewModel
    {
        #region Properties

        public string ClassName { get; set; }
        public string FadeIn { get; set; }
        public string fadeOut { get; set; }
        public string Delay { get; set; }
        public string Message { get; set; }
        #endregion
    }
}
