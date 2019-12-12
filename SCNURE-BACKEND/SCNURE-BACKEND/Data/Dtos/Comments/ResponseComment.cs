using SCNURE_BACKEND.Data.Dtos.Mappers;
using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Comments
{
    public class ResponseComment
    {
        public ResponseComment() { }

        public ResponseComment(User user, Comment comment)
        {
            CommentId = comment.CommentId;
            StartupId = comment.StartupId;
            UserInfo = UserMapper.ToUserInfoResponse(user);
            Text = comment.Text;
        }

        public int CommentId { get; set; }

        public int StartupId { get; set; }

        public UserInfo  UserInfo { get; set; }

        public string Text { get; set; }
    }
}
