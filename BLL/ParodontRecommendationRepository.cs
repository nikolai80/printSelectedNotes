using PrintSelected.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using PrintSelected.BLL;
using System.Windows;

namespace PrintSelected.BLL
{
    public class ParodontRecommendationRepository : IRepository<ParodontRecommendation>
    {
        private string path = @"ParodontRecommendation.json";

        public bool Create(string text)
        {
            var res = false;
            if (!String.IsNullOrEmpty(text))
            {
                Guid id = Guid.NewGuid();
                List<ParodontRecommendation> recommendationList = GetAll() ?? new List<ParodontRecommendation>();
                var recommendation = new ParodontRecommendation
                {
                    Id = id,
                    Text = text
                };
                recommendationList.Add(recommendation);

                try
                {
                    if (File.Exists(path))
                    {
                        string json = JsonConvert.SerializeObject(recommendationList, Formatting.Indented);
                        File.WriteAllText(path, json);
                        res = true;
                    }

                }
                catch (Exception ex)
                {

                    throw;
                } 
            }

            return res;
        }

        public List<ParodontRecommendation> GetAll()
        {
           return GetListData();
        }

        public ParodontRecommendation GetById(Guid id)
        {
            ParodontRecommendation res = new ParodontRecommendation();

            res = GetListData().Find(r => r.Id == id);

            return res;
        }

        public bool Remove(ParodontRecommendation item)
        {
            var res = false;

            return res;
        }

        private List<ParodontRecommendation> GetListData()
        {
            List<ParodontRecommendation> recommendationList = new List<ParodontRecommendation>();
            try
            {
                using (TextReader file = File.OpenText(path))
                {
                    string json = file.ReadToEnd();
                    JsonSerializer serializer = new JsonSerializer();
                    recommendationList = JsonConvert.DeserializeObject<List<ParodontRecommendation>>(json);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return recommendationList;
        }
    }
}
