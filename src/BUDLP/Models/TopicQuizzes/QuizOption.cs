﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUDLP.Models.TopicQuizzes
{
    public class QuizOption
    {
        public int QuizOptionId { get; set; }
        public string QuizOptionText { get; set; }
        public int QuizId { get; set; }
        public bool Answer { get; set; }
    }
}
