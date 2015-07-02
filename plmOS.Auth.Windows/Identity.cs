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
    public class Identity : IIdentity
    {
        private WindowsIdentity WindowsIdentity;

        public String ID
        {
            get
            {
                return this.WindowsIdentity.User.Value;
            }
        }

        public String Type
        {
            get
            {
                return this.WindowsIdentity.AuthenticationType;
            }
        }

        public String Domain
        {
            get
            {
                String[] parts = this.WindowsIdentity.Name.Split('\\');

                if (parts.Length == 2)
                {
                    return parts[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public String Name
        {
            get
            {
                String[] parts = this.WindowsIdentity.Name.Split('\\');

                if (parts.Length == 2)
                {
                    return parts[1];
                }
                else
                {
                    return parts[0];
                }
            }
        }

        public Boolean IsAuthenticated
        {
            get
            {
                return this.WindowsIdentity.IsAuthenticated;
            }
        }

        public bool Equals(IIdentity other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.ID.Equals(other.ID);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Identity)
            {
                return this.Equals((Identity)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public override string ToString()
        {
            return this.Domain + "\\" + this.Name;
        }

        internal Identity(WindowsIdentity WindowsIdentity)
        {
            this.WindowsIdentity = WindowsIdentity;
        }
    }
}
