using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match
{
    public partial class match : Component
    {
        public match()
        {
            InitializeComponent();
        }

        public match(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
