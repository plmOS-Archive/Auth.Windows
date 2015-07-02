/*  
  plmOS Auth Windows is a .NET library that implements Microsoft Windows plmOS Authentication.

  Copyright (C) 2015 Processwall Limited.

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as published
  by the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see http://opensource.org/licenses/AGPL-3.0.
 
  Company: Processwall Limited
  Address: The Winnowing House, Mill Lane, Askham Richard, York, YO23 3NW, United Kingdom
  Tel:     +44 113 815 3440
  Email:   support@processwall.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Threading;

namespace plmOS.Auth.Windows
{
    public class Manager : IManager
    {
        private Dictionary<String, Identity> IdentityCache;

        private Identity BuildIdentity(WindowsIdentity WindowsIdentity)
        {
            if (WindowsIdentity.IsAuthenticated)
            {
                String[] parts = WindowsIdentity.Name.Split('\\');

                if (!this.IdentityCache.ContainsKey(WindowsIdentity.User.AccountDomainSid.Value))
                {
                    this.IdentityCache[WindowsIdentity.User.AccountDomainSid.Value] = new Identity(WindowsIdentity.User.AccountDomainSid.Value, parts[0], parts[1]);
                }
                else
                {
                    this.IdentityCache[WindowsIdentity.User.AccountDomainSid.Value].Domain = parts[0];
                    this.IdentityCache[WindowsIdentity.User.AccountDomainSid.Value].Name = parts[1];
                }

                return this.IdentityCache[WindowsIdentity.User.AccountDomainSid.Value];
            }
            else
            {
                throw new LoginException();
            }
        }

        public IIdentity Login(ICredentials Credentials)
        {
            WindowsPrincipal principal = ((Windows.Credentials)Credentials).Principal;
            WindowsIdentity identity = (WindowsIdentity)principal.Identity;

            return this.BuildIdentity(identity);
        }

        public IIdentity CurrentIdentity
        {
            get
            {
                // Get Domain
                AppDomain domain = Thread.GetDomain();

                // Set Policy to Windows Principal
                domain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                // Get the Principal
                WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;

                // Get Identity
                WindowsIdentity identity = (WindowsIdentity)principal.Identity;

                return this.BuildIdentity(identity);
            }
        }

        public Manager()
        {
            this.IdentityCache = new Dictionary<String, Identity>();
        }
    }
}
