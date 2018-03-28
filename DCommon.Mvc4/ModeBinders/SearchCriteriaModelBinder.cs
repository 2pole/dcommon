using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DCommon;
using DCommon.Search;

namespace DCommon.Mvc.ModeBinders
{
    public class SearchCriteriaModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.IsInterface)
                return null;

            var model = base.BindModel(controllerContext, bindingContext);
            BindSearchCriterial(model, controllerContext, bindingContext);
            return model;
        }

        private void BindSearchCriterial(object model, ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var searchCriteriaModel = model as SearchCriteria;
            if (searchCriteriaModel != null)
            {
                var context = new ModelBindingContext
                    {
                        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => new SearchRequest(), typeof(SearchRequest)),
                        ModelState = bindingContext.ModelState,
                        PropertyFilter = bindingContext.PropertyFilter,
                        ValueProvider = bindingContext.ValueProvider
                    };

                var searchRequest = (SearchRequest)base.BindModel(controllerContext, context);
                if (searchRequest != null)
                {
                    searchRequest.Update(searchCriteriaModel);
                }
            }
        }
    }
}
