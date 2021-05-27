﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 18;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _pageSize) ? MaxPageSize : value;
        }

        public string CurrentUsername { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 17;
        public int MaxAge { get; set; }

        public string OrderBy { get; set; } = "lastActive";
    }
}

