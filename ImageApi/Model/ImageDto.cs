using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ImageApi.Model
{
    public class ImageDto : IErrorable
    {
        public string Id { get; set; }
        public IEnumerable<ImageParameter> Parameters { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public bool IsSucces { get; set; }

        public IList<string> ErrorList
        {
            get => _errorList ?? (_errorList = new List<string>());
            set => _errorList = value;
        }

        private IList<string> _errorList;
    }
}
