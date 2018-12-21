using AutoMapper;
using System;
using System.Linq;
using System.Web.Http;
using SMAPP.Dtos;
using SMAPP.Models;

namespace SMAPP.Controllers.Api
{
    public class SponsorsController : ApiController
    {
        private ApplicationDbContext _context;

        public SponsorsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/sponsors
        public IHttpActionResult GetSponsors(string query = null)
        {
            var sponsorsQuery = _context.Sponsors;

            var sponsorDtos = sponsorsQuery
                .ToList()
                .Select(Mapper.Map<Sponsor, SponsorDto>);

            return Ok(sponsorDtos);
        }

        // GET /api/sponsors/1
        public IHttpActionResult GetSponsor(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsor == null)
                return NotFound();

            return Ok(Mapper.Map<Sponsor, SponsorDto>(sponsor));
        }

        // POST /api/sponsors
        [HttpPost]
        public IHttpActionResult CreateSponsor(SponsorDto sponsorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var sponsor = Mapper.Map<SponsorDto, Sponsor>(sponsorDto);
            _context.Sponsors.Add(sponsor);
            _context.SaveChanges();

            sponsorDto.Id = sponsor.Id;
            return Created(new Uri(Request.RequestUri + "/" + sponsorDto.Id), sponsorDto);
        }

        // PUT /api/sponsors/1
        [HttpPut]
        public IHttpActionResult UpdateSponsor(int id, SponsorDto sponsorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var sponsorInDb = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsorInDb == null)
                return NotFound();

            Mapper.Map(sponsorDto, sponsorInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public IHttpActionResult DeleteSponsors(int id)
        {
            var sponsorDto = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsorDto == null)
                return NotFound();

            _context.Sponsors.Remove(sponsorDto);
            _context.SaveChanges();

            return Ok();
        }

    }
}

