using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFTTAPI.Model;

namespace TFTTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalentController : ControllerBase
    {
        private readonly TalentContext _rdsContext;
        public TalentController(TalentContext talentContext)
        {
            _rdsContext = talentContext;

        }

        [HttpGet("GetAllTalent")]
        public IActionResult GetAllTalent()
        {
            List<Talent> talentList = new List<Talent>();
            List<object> resultList = new List<object>();
            try
            {
                talentList = _rdsContext.Talents.ToList();
               resultList = new List<object>();

                foreach (Talent t in talentList)
                {
                    resultList.Add(new
                    {
                        id = t.Id,
                        name = t.Name,
                        shortName = t.ShortName,
                        profile = t.Profile,
                        bio = t.Bio,
                        reknown = t.Reknown

                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.Message);
            }


            return new JsonResult(resultList);
        }

        [HttpGet("GetTalentById/{id:int}")]
        public IActionResult GetTalentById(int id)
        {
            Talent t = _rdsContext.Talents.SingleOrDefault(x => x.Id == id);
            if (t == null)
            {
                return BadRequest();
            }
            object result = new object();

            result = new
            {
                id = t.Id,
                name = t.Name,
                shortName = t.ShortName,
                profile = t.Profile,
                bio = t.Bio,
                reknown = t.Reknown
            };

            return new JsonResult(result);
        }

        [HttpPost("PostTalent")]
        public IActionResult PostTalent(Talent talent)
        {
            Talent newT = new Talent();

            newT.Name = talent.Name;
            newT.ShortName = talent.ShortName;
            newT.Profile = talent.Profile;
            newT.Bio = talent.Bio;
            newT.Reknown = talent.Reknown;

            try
            {
                _rdsContext.Add(newT);
                _rdsContext.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("PutTalent/{id:int}")]
        public IActionResult PutTalent(int id, Talent talent)
        {
            Talent tRecord = _rdsContext.Talents.SingleOrDefault(x => x.Id == id);
            if (tRecord == null)
            {
                return BadRequest();
            }

            if (talent.Name != null)
            {
                tRecord.Name = talent.Name;
            }

            if (talent.ShortName != null)
            {
                tRecord.ShortName = talent.ShortName;

            }
            if (talent.Profile != null)
            {
                tRecord.Profile = talent.Profile;
            }
            if (talent.Bio != null)
            {
                tRecord.Bio = talent.Bio;
            }
            if (talent.Reknown != null)
            {
                tRecord.Reknown = talent.Reknown;
            }


            try
            {
                _rdsContext.Talents.Update(talent);
                _rdsContext.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("DeleteTalent/{id:int}")]
        public IActionResult DeleteTalent(int id)
        {
            Talent talent = _rdsContext.Talents.SingleOrDefault(x => x.Id == id);
            if (talent == null)
            {
                return BadRequest();
            }

            try
            {
                _rdsContext.Remove(talent);
                _rdsContext.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
