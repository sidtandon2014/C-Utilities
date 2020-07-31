using System;
using System.Collections.Generic;
using System.Text;

namespace TopWordsCustomSkill
{
    class InputRecord
    {
        public class InputRecordData
        {
            public string MergedText { get; set; }
        }
        public string RecordId { get; set; }
        public InputRecordData Data { get; set; }
    }

    
}
