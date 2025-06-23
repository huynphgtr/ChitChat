using AutoMapper;
using ChitChatApp.Share.ViewModels.Chat;
using ChitChatApp.Share.ViewModels.Items;

namespace ChitChatApp.Share.Mapping;

public class DtoToViewModelProfile : Profile
{
    public DtoToViewModelProfile()
    {
        // This profile maps DTOs from ChitChatApp.Core to ViewModels in ChitChatApp.Share
        // Since we don't have the DTOs created yet, these mappings are placeholders
        
        // When the DTOs are created in ChitChatApp.Core, update these mappings accordingly
        
        // Example mappings (uncomment and modify when DTOs are available):
        
        // CreateMap<AppUserDto, ContactListItemViewModel>()
        //     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
        //     .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FullName) ? src.FullName : src.UserName));

        // CreateMap<ChatRoomDto, ChatRoomListItemViewModel>()
        //     .ForMember(dest => dest.ChatRoomId, opt => opt.MapFrom(src => src.Id))
        //     .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        // CreateMap<MessageDto, MessageViewModel>()
        //     .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
        //     .ForMember(dest => dest.IsFromCurrentUser, opt => opt.Ignore()); // This would be set separately based on current user context

        // Add more mappings as needed when DTOs are available
    }
}

