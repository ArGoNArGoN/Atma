using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public interface IChat
	{
		List<ServerUser> ServerUsers { get; }
	}
}
