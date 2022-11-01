using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using iwip.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;
using iwip.PO;

namespace iwip.Blazor.Menus;

public class iwipMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public iwipMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<iwipResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                iwipMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );

        context.Menu.AddItem(new ApplicationMenuItem(
                "iwip",
                l["Menu:PO"],
                icon: "fa fa-book")
                .AddItem(new ApplicationMenuItem(
                "iwip.PO",
                l["Menu:PO.List"],
                url: "/pos").RequirePermissions("PO.PO")
                )
        );

        context.Menu.AddItem(new ApplicationMenuItem(
            "iwip",
            l["Menu:Import"],
            icon: "fa fa-upload")
            .AddItem(new ApplicationMenuItem(
            "iwip.Import",
            l["Menu:Import.ImportData"],
            url: "/import").RequirePermissions("Import.Import")
        )
);

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AccountResource>();

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            l["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
