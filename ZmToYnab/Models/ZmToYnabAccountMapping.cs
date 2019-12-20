using System;

namespace ZmToYnab.Models
{
    public struct ZmToYnabAccountMapping
    {
        public Guid ZmAccountId {get;set;} 
        public Guid YnabAccountId {get;set;}
        public string AccountName {get;set;}
    }
}