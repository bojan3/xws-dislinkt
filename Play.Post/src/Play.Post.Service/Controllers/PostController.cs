using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Post.Service.Clients;
using Play.Post.Service.Dtos;
using Play.Post.Service.Entities;

namespace Play.Post.Service.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly IRepository<Play.Post.Service.Entities.Post> postsRepository;
        private readonly AccountClient accountClinet;


        // dodati AccountClient accountClient u kontruktor
        public PostController(IRepository<Play.Post.Service.Entities.Post> postsRepository)
        {
            this.postsRepository = postsRepository;
            //this.accountClinet = accountClient;
        }

        [HttpGet]
        public async Task<IEnumerable<PostDto>> GetAsync()
        {
            var posts = (await postsRepository.GetAllAsync()).Select(post => post.AsDto());
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

        // [HttpGet("/account/{id}")]
        // public async Task<IEnumerable<PostDto>> GetPostsByAccountIdAsync(Guid id)
        // {
        //     // var post = (await postsRepository.GetAsync(id)).AsDto();

        //     // if (post == null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // return post;

        //     var isPublic = (accountClinet.GetIsPublic(id)).Result;

        //     if (!isPublic.Value)
        //         return Enumerable.Empty<PostDto>();

        //     var posts = (await postsRepository.GetAllAsync()).Where(post => post.AccountId == id).Select(post => post.AsDto());

        //     return posts;
        // }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> postCommentAsync(Guid id, CommentDto commentDto)
        {
            var post = await postsRepository.GetAsync(id);

            var comment = new Comment
            {
                AccountId = commentDto.AccountId,
                Text = commentDto.Text,
            };

            post.Comments.Add(comment);

            await postsRepository.UpdateAsync(post);

            return NoContent();
        }
    }
}