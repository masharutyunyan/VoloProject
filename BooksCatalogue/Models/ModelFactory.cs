using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksCatalogue.Models
{
    public class ModelFactory
    {
        public static ModelFactory Create()
        {
            return new ModelFactory();
        }

        }
}