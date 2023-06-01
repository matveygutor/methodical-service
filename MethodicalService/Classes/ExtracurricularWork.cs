using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodicalService.MethodicalWorkForms
{
    partial class ExtracurricularWork
    {
        public ExtracurricularWork(int id, string view, string placeOfProvision, string deadLines, bool completed) 
        {
            ID = id;
            View = view;
            PlaceOfProvision = placeOfProvision;
            DeadLines = deadLines;
            Completed = completed;
        }

        public int ID { get; set; }
        public string View { get; set; }
        public string PlaceOfProvision { get; set; }
        public string DeadLines { get; set; }
        public bool Completed { get; set; }
    }
}
