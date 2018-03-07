using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TVController.BE1.Models
{
    public class Entry
    {
        public EntryType EntryType { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1} - {2} ({3})",Name, Extension, EntryType, FullName);
        }
    }
    public enum EntryType
    {
        Parent = 0,
        Directory = 1,
        Movie = 2,
        Subtitle = 3,
        OtherFile = 4
    }

    public class EntriesData
    {
        public string EntryPath { get; set; }
        public List<Entry> Entries { get; set; }
    }
}