//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebProject1.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class actorInMovie
    {
        public int id { get; set; }
        public Nullable<int> actorid { get; set; }
        public Nullable<int> movieid { get; set; }
    
        public virtual actor actor { get; set; }
        public virtual movies movies { get; set; }
    }
}
