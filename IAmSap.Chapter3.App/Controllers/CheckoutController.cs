using System;
using IAmSap.Chapter3.App.Models.Config;
using IAmSap.Chapter3.App.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace IAmSap.Chapter3.App.Controllers
{
    public class CheckoutController : Controller
    {
        private IFeatureManager _featureManager;
        private readonly IOptionsSnapshot<EcommerceConfig> _config;

        public CheckoutController(IFeatureManager featureManager, IOptionsSnapshot<EcommerceConfig> config)
        {
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
            _config = config;
        }

        public IActionResult Index()
        {
            return View(new CheckoutViewModel
            {
                IsDiscountEnabled = _featureManager.IsEnabled("DiscountCodes"),
                // Getting a Key Vaul secret via App Configuration:
                // 1 Set the secret on the Key Vault
                // 2 Access the secret from App Configuration, in this case we are using a Key named "EcommerceConfig:SecretDiscountCode" 
                // that is related to the secret on Key Vault
                SecretDiscountCode = _config.Value.SecretDiscountCode
            });
        }

    }
}