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
        private readonly string path = @"ParodontRecommendation.json";

        public bool Create(string text)
        {
            var res = false;
            if (!String.IsNullOrEmpty(text))
            {
                Guid id = Guid.NewGuid();
                List<ParodontRecommendation> recommendationList = GetListData() ?? new List<ParodontRecommendation>();
                var recommendation = new ParodontRecommendation
                {
                    Id = id,
                    Text = text
                };
                recommendationList.Add(recommendation);
                res = UpdateFileData(recommendationList);
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

        public void Remove(List<Guid> listId)
        {
            
            List<ParodontRecommendation> recommendationList = GetListData() ?? new List<ParodontRecommendation>();
            foreach (var id in listId)
            {
                recommendationList.Remove(recommendationList.Find(r => r.Id == id)); 
            }
            this.UpdateFileData(recommendationList);
            
        }

        public bool Update(Guid id, string text)
        {
            var res = false;
            if (!String.IsNullOrEmpty(text))
            {
                List<ParodontRecommendation> recommendationList = GetListData() ?? new List<ParodontRecommendation>();
                recommendationList.Find(r => r.Id == id).Text = text;
                res = UpdateFileData(recommendationList);
            }

            return res;
        }

        private List<ParodontRecommendation> GetListData()
        {
            List<ParodontRecommendation> recommendationList = new List<ParodontRecommendation>();
            try
            {
                using (TextReader file = File.OpenText(path))
                {
                    var json = file.ReadToEnd();
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

        private bool UpdateFileData(List<ParodontRecommendation> recommendationList)
        {
            var res = false;
            try
            {
                if (File.Exists(path))
                {
                    var json = JsonConvert.SerializeObject(recommendationList, Formatting.Indented);
                    File.WriteAllText(path, json);
                    res = true;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return res;
        }

    }
}
