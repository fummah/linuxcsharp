using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aaa_library
{
    public abstract class amountsGrouper
    {
        public List<myGroup> processGroup(List<myGroup> source_list)
        {

            List<myGroup> mylist = source_list.GroupBy(l => new
            {
                claim_number = l.claim_number,
                practice_number = l.practice_number
            }).Select(cl => new myGroup()
            {
                claim_number = cl.First<myGroup>().claim_number,
                practice_number = cl.First<myGroup>().practice_number,
                charged_amnt = cl.First<myGroup>().charged_amnt,
                scheme = cl.First<myGroup>().scheme,
                gap = cl.First<myGroup>().gap
            }).ToList<myGroup>().GroupBy<myGroup, string>((Func<myGroup, string>)(l => l.claim_number)).Select<IGrouping<string, myGroup>, myGroup>((Func<IGrouping<string, myGroup>, myGroup>)(cl => new myGroup()
            {
                claim_number = cl.First<myGroup>().claim_number,
                charged_amnt = cl.Sum<myGroup>((Func<myGroup, double>)(c => c.charged_amnt)),
                scheme = cl.Sum<myGroup>((Func<myGroup, double>)(c => c.scheme)),
                gap = cl.Sum<myGroup>((Func<myGroup, double>)(c => c.gap))
            })).ToList<myGroup>();

            return mylist;
        }
        public List<splitGroup> processGroupSplit(List<splitGroup> source_list)
        {
            List<splitGroup> mylist = source_list.GroupBy(l => l.loyalty_number)
    .Select(cl => new splitGroup
    {
        loyalty_number = cl.First().loyalty_number,
        charged_amount = cl.Sum(c => c.charged_amount),
        scheme_amount = cl.Sum(c => c.scheme_amount),
        copayment = cl.Sum(c => c.copayment),
    }).ToList();
           
            return mylist;
        }
        protected string chechInput1(string str)
        {

            if (str == "")
            {
                str = "";
            }

            return str;
        }
        protected string chechAmount1(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                str = "0.0";
            }
            else
            {
                str = str.Replace(',', '.');
            }
            return str;
        }
        protected string chechAmount2(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                str = "0.0";
            }
            else
            {
                str = str.Replace('.', ',');
            }
            return str;
        }

    }
    public class myGroup
    {
        public string claim_number { get; set; }
        public string practice_number { get; set; }
        public double charged_amnt { get; set; }
        public double scheme { get; set; }
        public double gap { get; set; }
    }
    public class splitGroup
    {
        public string loyalty_number { get; set; }     
       
        public double charged_amount { get; set; }
        public double scheme_amount { get; set; }
        public int copayment { get; set; }
       
    }
}
