//Keith Russell
// 16.12.18
// using an MVC controller try to check to see if declarationcommand and siteID exists and check whether their value were <Default> then change them to -1,
// if SiteID = <>'DUB' then -2
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using XMLWebService.Models;

namespace XMLWebService.Controllers
{
    public class InputDocumentController : Controller
    {
        // GET: InputDocument
        
        public ActionResult DisplayXML()
        {
            var data = new List<InputDocumentModel>();

            data = ReturnData();

            return View(data);

        }

        public List<InputDocumentModel> ReturnData()

        {
            //
            string xmldata = Server.MapPath("~/XMLFile/InputDocument.xml");

            // test to see if document has passed
            using (XmlReader xr = XmlReader.Create(
                new StringReader(xmldata)))
            {
                try
                {
                    while (xr.Read()) { }
                    Console.WriteLine("Pass");
                    InputDocumentModel.documentStatus = 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail: " + ex.Message);

                }

            }
            XmlDocument doc = new XmlDocument();
            doc.Load("~/ XMLFile / InputDocument.xml");
            XmlNodeList xnListDefault = doc.SelectNodes("/InputDocument/DeclarationList/Declaration[@Command]");
            XmlNodeList xnListsiteID = doc.SelectNodes("/InputDocument/DeclarationList/Declaration/DeclarationHeader/DeclarationDestination/SiteID");
            if (xnListDefault.InnerText !='DEFAULT')
            {
                InputDocumentModel.DocumentStatus = -1;
            }
            
            if (xnListsiteID.InnerText != "DUB")
            {
                InputDocumentModel.DocumentStatus = -2;
            }

             


            // check the other details
            DataSet ds = new DataSet();

                ds.ReadXml(xmldata);

            

            // put the strings into a list
            var inputDocumentList = new List<InputDocumentModel>();
            inputDocumentList = (from rows in ds.Tables[0].AsEnumerable()
                                 select new InputDocumentModel
                                 {
                                     declarationCommand = rows[4].ToString(),
                                     siteID = rows[10].ToString()
                                 }).ToList();

            
            return inputDocumentList;

        }
    }
}


