﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public class ProjectTranslateDTO
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }
        public int ProjectId { get; set; }

        [Required, StringLength(100)]
        public string ProjectName { get; set; }
        public string Subtitle { get; set; }


        public string Content { get; set; }

    }
}
