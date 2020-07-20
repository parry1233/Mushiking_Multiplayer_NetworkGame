using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace WpfApp_NetworkGame
{
	class xmlRun
	{
		private Dictionary<string, string[]> allCards = new Dictionary<string, string[]>();
		public xmlRun()
		{

		}

		public void LoadCard()
		{
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load("../../DataXML/Card.xml");
				XmlNodeList Cards = xmlDoc.SelectNodes("Cards/Card");

				foreach(XmlNode card in Cards)
				{
					string[] aCard = new string[4];
					aCard[0] = card["ID"].InnerText;
					aCard[1] = card["Name"].InnerText;
					aCard[2] = card["Pts"].InnerText;
					aCard[3] = card["Type"].InnerText;

					this.allCards.Add(aCard[0], aCard);
				}
			}
			catch(Exception e)
			{
				MessageBox.Show("XML_ERR: " + e.Message);
			}
		}

		public string[] getCardInfo(string id)
		{
			bool check=false;
			string[] card_tmp = new string[4];
			foreach(KeyValuePair<string,string[]> card in this.allCards)
			{
				if(card.Key.Equals(id))
				{
					check = true;
					card_tmp = card.Value;
				}
			}
			if(!check)
			{
				return null;
			}
			else
			{
				return card_tmp;
			}
		}

		public string getRandom()
		{
			List<string> ssrList = new List<string>();
			List<string> srList = new List<string>();
			List<string> rList = new List<string>();
			List<string> nList = new List<string>();
			int ssr = 0;
			int sr = 0;
			int r = 0;
			int n = 0;

			foreach (KeyValuePair<string, string[]> card in this.allCards)
			{
				if (card.Value[2].Equals("300"))
				{
					ssr++;
					ssrList.Add(card.Value[0]);
				}
				else if(card.Value[2].Equals("260"))
				{
					sr++;
					srList.Add(card.Value[0]);
				}
				else if (card.Value[2].Equals("210"))
				{
					r++;
					rList.Add(card.Value[0]);
				}
				else if (card.Value[2].Equals("180"))
				{
					n++;
					nList.Add(card.Value[0]);
				}
			}

			int sampleCount = 100;
			int percentSSR = 5;
			int percentSR = 10;
			int percentR = 40;
			int percentN = 45;

			Random rand1 = new Random(Guid.NewGuid().GetHashCode());
			int dice = rand1.Next(0, sampleCount);
			if (dice<=percentN)
			{
				//N
				Random rand2 = new Random(Guid.NewGuid().GetHashCode());
				return nList[rand2.Next(0, n)];
			}
			else if(dice>percentN&&dice<=(percentN+percentR))
			{
				//R
				Random rand2 = new Random(Guid.NewGuid().GetHashCode());
				return rList[rand2.Next(0, r)];
			}
			else if(dice> (percentN + percentR)&&dice<= (percentN + percentR+percentSR))
			{
				//SR
				Random rand2 = new Random(Guid.NewGuid().GetHashCode());
				return srList[rand2.Next(0, sr)];
			}
			else if(dice > (percentN + percentR + percentSR) && dice <= (percentN + percentR + percentSR +percentSSR))
			{
				//SSR
				Random rand2 = new Random(Guid.NewGuid().GetHashCode());
				return ssrList[rand2.Next(0, ssr)];
			}
			else
			{
				return "ERR";
			}
		}
	}
}
