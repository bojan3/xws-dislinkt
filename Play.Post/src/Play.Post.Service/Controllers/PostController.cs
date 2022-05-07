using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Post.Service.Dtos;

namespace Play.Post.Service.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly IRepository<Play.Post.Service.Entities.Post> postsRepository;

        public PostController(IRepository<Play.Post.Service.Entities.Post> postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<PostDto>> GetAsync()
        {

            var posts = (await postsRepository.GetAllAsync()).Select(account => account.AsDto());
            return posts;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetByIdAsync(Guid id)
        {
            var post = (await postsRepository.GetAsync(id)).AsDto();

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> PostAsync(CreateAccountPostDto createAccountPostDto)
        {
            var post = new Play.Post.Service.Entities.Post
            {
                AccountId = createAccountPostDto.AccountId,
                Text = createAccountPostDto.Text,
                Image = createAccountPostDto.Image,
                Link = createAccountPostDto.Link,
                LikeCount = 0,
                DislikeCount = 0,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await postsRepository.CreateAsync(post);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = post.Id }, post);
        }
    }
}