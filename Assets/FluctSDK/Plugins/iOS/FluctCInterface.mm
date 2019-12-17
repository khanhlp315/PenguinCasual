//
//  FluctCInterface.m
//  FluctCInterface
//
//  Created by KUROSAKI Ryota on 2014/01/28.
//  Copyright (c) 2014年 Zucks, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <FluctSDK/FluctSDK.h>

UIKIT_EXTERN CGRect CGRectMove(CGRect rect, CGFloat x, CGFloat y, CGFloat relativeX, CGFloat relativeY);
UIKIT_EXTERN CGFloat CGRectGetPointX(CGRect rect, CGFloat relativeX);
UIKIT_EXTERN CGFloat CGRectGetPointY(CGRect rect, CGFloat relativeY);
UIKIT_EXTERN CGPoint CGRectGetPoint(CGRect rect, CGFloat relativeX, CGFloat relativeY);
UIKIT_EXTERN CGRect CGRectContentMode(CGRect rect, CGRect parentRect, UIViewContentMode contentMode);

#ifdef __cplusplus
extern "C" {
#endif

#pragma mark - FluctPlugin_RewardedVideo_Handler(Delegate)
    typedef void (*DidLoadHandlerFunc)(const char *, const char *);
    static DidLoadHandlerFunc DidLoadHandler;
    void _FluctPlugin_RewardedVideo_SetDidLoadHandler(DidLoadHandlerFunc handler)
    {
        DidLoadHandler = handler;
    }

    typedef void (*DidAppearHandlerFunc)(const char *, const char *);
    static DidAppearHandlerFunc DidAppearHandler;
    void _FluctPlugin_RewardedVideo_SetDidAppearHandler(DidAppearHandlerFunc handler)
    {
        DidAppearHandler = handler;
    }

    typedef void (*DidDisappearHandlerFunc)(const char *, const char *);
    static DidDisappearHandlerFunc DidDisappearHandler;
    void _FluctPlugin_RewardedVideo_SetDidDisappearHandler(DidDisappearHandlerFunc handler)
    {
        DidDisappearHandler = handler;
    }

    typedef void (*DidDisappearHandlerFunc)(const char *, const char *);
    static DidDisappearHandlerFunc ShouldRewardHandler;
    void _FluctPlugin_RewardedVideo_SetShouldRewardHandler(DidDisappearHandlerFunc handler)
    {
        ShouldRewardHandler = handler;
    }

    typedef void (*DidFailToLoadHandlerFunc)(const char *, const char *, const char *, const char *);
    static DidFailToLoadHandlerFunc DidFailToLoadHandler;
    void _FluctPlugin_RewardedVideo_SetDidFailToLoadHandler(DidFailToLoadHandlerFunc handler)
    {
        DidFailToLoadHandler = handler;
    }

    typedef void (*DidFailToPlayHandlerFunc)(const char *, const char *, const char *, const char *);
    static DidFailToPlayHandlerFunc DidFailToPlayHandler;
    void _FluctPlugin_RewardedVideo_SetDidFailToPlayHandler(DidFailToPlayHandlerFunc handler)
    {
        DidFailToPlayHandler = handler;
    }
#ifdef __cplusplus
}
#endif

inline CGFloat CGRectGetPointX(CGRect rect, CGFloat relativeX)
{
    return rect.origin.x + rect.size.width * relativeX;
}

inline CGFloat CGRectGetPointY(CGRect rect, CGFloat relativeY)
{
    return rect.origin.y + rect.size.height * relativeY;
}

inline CGPoint CGRectGetPoint(CGRect rect, CGFloat relativeX, CGFloat relativeY)
{
    return CGPointMake(CGRectGetPointX(rect, relativeX),
                       CGRectGetPointY(rect, relativeY));
}

inline CGRect CGRectMove(CGRect rect, CGFloat x, CGFloat y, CGFloat relativeX, CGFloat relativeY)
{
    CGPoint relativePoint = CGRectGetPoint(rect, relativeX, relativeY);
    CGRect moveRect = rect;
    moveRect.origin.x += x - relativePoint.x;
    moveRect.origin.y += y - relativePoint.y;
    return moveRect;
}

CGRect CGRectContentMode(CGRect rect, CGRect parentRect, UIViewContentMode contentMode)
{
    CGRect result = rect;
    
    if (contentMode == UIViewContentModeScaleToFill) {
        result = parentRect;
    }
    else if (contentMode == UIViewContentModeScaleAspectFit) {
        if ((parentRect.size.width / parentRect.size.height) > (rect.size.width / rect.size.height)) {
            // rectの高さをparentRectの高さに揃えるようにリサイズする
            result.size = CGSizeMake(parentRect.size.height * rect.size.width / rect.size.height,
                                     parentRect.size.height);
        }
        else {
            // rectの幅をparentRectの幅に揃えるようにリサイズする
            result.size = CGSizeMake(parentRect.size.width,
                                     parentRect.size.width * rect.size.height / rect.size.width);
        }
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.5),
                            CGRectGetPointY(parentRect, 0.5),
                            0.5, 0.5);
    }
    else if (contentMode == UIViewContentModeScaleAspectFill) {
        if ((parentRect.size.width / parentRect.size.height) > (rect.size.width / rect.size.height)) {
            // rectの幅をparentRectの幅に揃えるようにリサイズする
            result.size = CGSizeMake(parentRect.size.width,
                                     parentRect.size.width * rect.size.height / rect.size.width);
        }
        else {
            // rectの高さをparentRectの高さに揃えるようにリサイズする
            result.size = CGSizeMake(parentRect.size.height * rect.size.width / rect.size.height,
                                     parentRect.size.height);
        }
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.5),
                            CGRectGetPointY(parentRect, 0.5),
                            0.5, 0.5);
    }
    else if (contentMode == UIViewContentModeRedraw) {
        // nothing to do
    }
    else if (contentMode == UIViewContentModeCenter) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.5),
                            CGRectGetPointY(parentRect, 0.5),
                            0.5, 0.5);
    }
    else if (contentMode == UIViewContentModeTop) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.5),
                            CGRectGetPointY(parentRect, 0.0),
                            0.5, 0.0);
    }
    else if (contentMode == UIViewContentModeBottom) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.5),
                            CGRectGetPointY(parentRect, 1.0),
                            0.5, 1.0);
    }
    else if (contentMode == UIViewContentModeLeft) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.0),
                            CGRectGetPointY(parentRect, 0.5),
                            0.0, 0.5);
    }
    else if (contentMode == UIViewContentModeRight) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 1.0),
                            CGRectGetPointY(parentRect, 0.5),
                            1.0, 0.5);
    }
    else if (contentMode == UIViewContentModeTopLeft) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.0),
                            CGRectGetPointY(parentRect, 0.0),
                            0.0, 0.0);
    }
    else if (contentMode == UIViewContentModeTopRight) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 1.0),
                            CGRectGetPointY(parentRect, 0.0),
                            1.0, 0.0);
    }
    else if (contentMode == UIViewContentModeBottomLeft) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 0.0),
                            CGRectGetPointY(parentRect, 1.0),
                            0.0, 1.0);
    }
    else if (contentMode == UIViewContentModeBottomRight) {
        result = CGRectMove(result,
                            CGRectGetPointX(parentRect, 1.0),
                            CGRectGetPointY(parentRect, 1.0),
                            1.0, 1.0);
    }
    
    return result;
}

typedef enum FluctBannerViewPosition {
    FluctBannerViewPositionLeft             = 0x1 << 0,
    FluctBannerViewPositionTop              = 0x1 << 1,
    FluctBannerViewPositionRight            = 0x1 << 2,
    FluctBannerViewPositionBottom           = 0x1 << 3,
    FluctBannerViewPositionCenterVertical   = 0x1 << 4,
    FluctBannerViewPositionCenterHorizontal = 0x1 << 5,
} FluctBannerViewPosition;

extern UIViewController *UnityGetGLViewController();
extern void UnitySendMessage(const char *, const char *, const char *);

@interface FluctCInterface : NSObject <FSSRewardedVideoDelegate>

@property (nonatomic, strong) NSMutableDictionary *bannerDictionary;
@property (nonatomic, strong) NSMutableDictionary *interstitialDictionary;

@end

@implementation FluctCInterface

+ (FluctCInterface *)sharedObject
{
    static dispatch_once_t onceToken;
    static FluctCInterface *_sharedObject;
    dispatch_once(&onceToken, ^{
        _sharedObject = [[FluctCInterface alloc] init];
        FSSRewardedVideo.sharedInstance.delegate = _sharedObject;
        
        FSSRewardedVideoSetting *setting = FSSRewardedVideoSetting.defaultSetting;
        setting.activation.adMobActivated = NO;
        FSSRewardedVideo.sharedInstance.setting = setting;
    });
    return _sharedObject;
}

- (id)init
{
    self = [super init];
    if (self) {
        _bannerDictionary = @{}.mutableCopy;
        _interstitialDictionary = @{}.mutableCopy;
    }
    return self;
}

- (BOOL)bannerExistWithID:(int)bannerObjectID
{
    return (self.bannerDictionary[@(bannerObjectID)] != nil);
}

- (BOOL)interstitialExistWithID:(int)interstitialObjectID
{
    return (self.interstitialDictionary[@(interstitialObjectID)] != nil);
}

- (FSSBannerView *)bannerWithID:(int)bannerObjectID
{
    return self.bannerDictionary[@(bannerObjectID)];
}

- (FSSInterstitialView *)interstitialWithID:(int)interstitialObjectID
{
    return self.interstitialDictionary[@(interstitialObjectID)];
}

- (BOOL)setBanner:(FSSBannerView *)bannerView forID:(int)bannerObjectID
{
    if (![self bannerExistWithID:bannerObjectID]) {
        self.bannerDictionary[@(bannerObjectID)] = bannerView;
        return YES;
    }
    return NO;
}

- (BOOL)setInterstitial:(FSSInterstitialView *)interstitialView forID:(int)interstitialObjectID
{
    if (![self interstitialExistWithID:interstitialObjectID]) {
        self.interstitialDictionary[@(interstitialObjectID)] = interstitialView;
        return YES;
    }
    return NO;
}

- (BOOL)removeBannerForID:(int)bannerObjectID
{
    if ([self bannerExistWithID:bannerObjectID]) {
        [self.bannerDictionary removeObjectForKey:@(bannerObjectID)];
        return YES;
    }
    return NO;
}

- (BOOL)removeInterstitialForID:(int)interstitialObjectID
{
    if ([self interstitialExistWithID:interstitialObjectID]) {
        [self.interstitialDictionary removeObjectForKey:@(interstitialObjectID)];
        return YES;
    }
    return NO;
}

#pragma mark - RewardedVideo
- (void)loadFluctRewardedVideo:(NSString *)groupId unitId:(NSString *)unitId
{
    [[FSSRewardedVideo sharedInstance] loadRewardedVideoWithGroupId: groupId unitId: unitId];
}

- (void)loadFluctRewardedVideo:(NSString *)groupId unitId:(NSString *)unitId userId:(NSString *)userId gender:(int)gender birthday:(NSString *)birthday age:(int)age
{
    FSSAdRequestTargeting *targeting = [FSSAdRequestTargeting new];
    if (birthday != nil) {
        NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
        [formatter setDateFormat:@"yyyy-MM-dd"];
        [formatter setTimeZone:[NSTimeZone timeZoneForSecondsFromGMT:0]];
        targeting.birthday = [formatter dateFromString:birthday];
    }
    targeting.userID = userId;
    targeting.gender = (FSSGender) gender;
    targeting.age = age;
    [[FSSRewardedVideo sharedInstance] loadRewardedVideoWithGroupId: groupId unitId: unitId targeting: targeting];
}

- (void)presentFluctRewardedVideo:(NSString *)groupId unitId:(NSString *)unitId
{
    [[FSSRewardedVideo sharedInstance] presentRewardedVideoAdForGroupId: groupId unitId: unitId fromViewController:[[[[UIApplication sharedApplication] windows] firstObject] rootViewController]];
}

- (BOOL)hasAdAvailableFluctRewardedVideo:(NSString *)groupId unitId:(NSString *)unitId
{
    return [[FSSRewardedVideo sharedInstance] hasAdAvailableForGroupId:groupId unitId:unitId];
}

#pragma mark - RewardedVideoDelegate
- (void)rewardedVideoDidLoadForGroupID:(NSString *)groupId unitId:(NSString *)unitId
{
    DidLoadHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String]);
}

- (void)rewardedVideoDidFailToLoadForGroupId:(NSString *)groupId unitId:(NSString *)unitId error:(NSError *)error
{
    NSString *errorCode = [NSString stringWithFormat:@"%ld", error.code];
    NSString *errorMessage = [error.userInfo objectForKey:NSLocalizedDescriptionKey];
    DidFailToLoadHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String], (char *)[errorCode UTF8String], (char *)[errorMessage UTF8String]);
}

- (void)rewardedVideoDidFailToPlayForGroupId:(NSString *)groupId unitId:(NSString *)unitId error:(NSError *)error
{
    NSString *errorCode = [NSString stringWithFormat:@"%ld", error.code];
    NSString *errorMessage = [error.userInfo objectForKey:NSLocalizedDescriptionKey];
    DidFailToPlayHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String], (char *)[errorCode UTF8String], (char *)[errorMessage UTF8String]);
}

- (void)rewardedVideoDidAppearForGroupId:(NSString *)groupId unitId:(NSString *)unitId
{
    DidAppearHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String]);
}

- (void)rewardedVideoDidDisappearForGroupId:(NSString *)groupId unitId:(NSString *)unitId
{
    DidDisappearHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String]);
}

- (void)rewardedVideoShouldRewardForGroupID:(NSString *)groupId unitId:(NSString *)unitId
{
    ShouldRewardHandler((char *)[groupId UTF8String], (char *)[unitId UTF8String]);
}

@end

#ifdef __cplusplus
extern "C" {
#endif

#pragma mark - FluctSDK
    void _FluctPlugin_Configure(char *unityVersion, char *bridgePluginVersion) {
        [FluctCInterface sharedObject];
        FSSConfigurationOptions *options = [FSSConfigurationOptions new];
        options.envType = FSSDevelopmentEnvironmentTypeUnity;
        options.bridgePluginVersion = @(bridgePluginVersion);
        options.envVersion = @(unityVersion);
        [FluctSDK configureWithOptions:options];
    }

void FluctSDKSetBannerConfiguration(char *media_id)
{
    NSString *mediaID = [NSString stringWithCString:media_id encoding:NSUTF8StringEncoding];
    [[FluctSDK sharedInstance] setBannerConfiguration:mediaID];
}

#pragma mark - FluctBannerView

bool FluctBannerViewCreate(int object_id, char *media_id)
{
    if (![[FluctCInterface sharedObject] bannerExistWithID:object_id]) {
        FSSBannerView *view =  [[FSSBannerView alloc] initWithFrame:CGRectMake(0, 0, 320, 50)];
        if (media_id != NULL && strlen(media_id) > 0) {
            [view setMediaID:@(media_id)];
        }
        [[FluctCInterface sharedObject] setBanner:view forID:object_id];
        return true;
    }
    return false;
}

bool FluctBannerViewDestroy(int object_id)
{
    return [[FluctCInterface sharedObject] removeBannerForID:object_id];
}

bool FluctBannerViewExist(int object_id)
{
    return [[FluctCInterface sharedObject] bannerExistWithID:object_id];
}

bool FluctBannerViewSetMediaID(int object_id, char *media_id)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        if (media_id != NULL && strlen(media_id) > 0) {
            [view setMediaID:@(media_id)];
            return true;
        }
    }
    return false;
}

bool FluctBannerViewGetFrame(int object_id, float *x, float *y, float *width, float *height)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        CGRect frame = view.frame;
        if (x != NULL) {
            *x = frame.origin.x;
        }
        if (y != NULL) {
            *y = frame.origin.y;
        }
        if (width != NULL) {
            *width = frame.size.width;
        }
        if (height != NULL) {
            *height = frame.size.height;
        }
        return true;
    }
    return false;
}

bool FluctBannerViewSetFrame(int object_id, float x, float y, float width, float height)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        view.frame = CGRectMake(x, y, width, height);
        return true;
    }
    return false;
}

bool FluctBannerViewSetPosition(int object_id, float width, float height, int position, float left, float top, float right, float bottom)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        int adjustPosition = 0;
        if (position & FluctBannerViewPositionLeft) {
            adjustPosition |= FluctBannerViewPositionLeft;
        }
        else if (position & FluctBannerViewPositionRight) {
            adjustPosition |= FluctBannerViewPositionRight;
        }
        else if (position & FluctBannerViewPositionCenterHorizontal) {
            adjustPosition |= FluctBannerViewPositionCenterHorizontal;
        }
        else {
            adjustPosition |= FluctBannerViewPositionLeft;
        }
        
        if (position & FluctBannerViewPositionTop) {
            adjustPosition |= FluctBannerViewPositionTop;
        }
        else if (position & FluctBannerViewPositionBottom) {
            adjustPosition |= FluctBannerViewPositionBottom;
        }
        else if (position & FluctBannerViewPositionCenterVertical) {
            adjustPosition |= FluctBannerViewPositionCenterVertical;
        }
        else {
            adjustPosition |= FluctBannerViewPositionTop;
        }
        
        CGRect frame = CGRectMake(0.0f, 0.0f, width, height);
        UIViewController *c = UnityGetGLViewController();

        UIViewContentMode mode = UIViewContentModeTopLeft;
        if (adjustPosition & FluctBannerViewPositionLeft) {
            if (adjustPosition & FluctBannerViewPositionTop) {
                mode = UIViewContentModeTopLeft;
            }
            else if (adjustPosition & FluctBannerViewPositionCenterVertical) {
                mode = UIViewContentModeLeft;
            }
            else if (adjustPosition & FluctBannerViewPositionBottom) {
                mode = UIViewContentModeBottomLeft;
            }
        }
        else if (adjustPosition & FluctBannerViewPositionCenterHorizontal) {
            if (adjustPosition & FluctBannerViewPositionTop) {
                mode = UIViewContentModeTop;
            }
            else if (adjustPosition & FluctBannerViewPositionCenterVertical) {
                mode = UIViewContentModeCenter;
            }
            else if (adjustPosition & FluctBannerViewPositionBottom) {
                mode = UIViewContentModeBottom;
            }
        }
        else if (adjustPosition & FluctBannerViewPositionRight) {
            if (adjustPosition & FluctBannerViewPositionTop) {
                mode = UIViewContentModeTopRight;
            }
            else if (adjustPosition & FluctBannerViewPositionCenterVertical) {
                mode = UIViewContentModeRight;
            }
            else if (adjustPosition & FluctBannerViewPositionBottom) {
                mode = UIViewContentModeBottomRight;
            }
        }
        
        frame = CGRectContentMode(frame, c.view.bounds, mode);
        frame.origin.x += left + (-1 * right);
        frame.origin.y += top + (-1 * bottom);
        FluctBannerViewSetFrame(object_id, frame.origin.x, frame.origin.y, frame.size.width, frame.size.height);
        return true;
    }
    return false;
}

bool FluctBannerViewShow(int object_id)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        UIViewController *c = UnityGetGLViewController();
        BOOL chk = YES;
        for (UIView *v in [c.view subviews]) {
          if (v == view) {
            chk = NO;
          }
        }
        if (chk) {
          [c.view addSubview:view];
        }
        return true;
    }
    return false;
}

bool FluctBannerViewDismiss(int object_id)
{
    FSSBannerView *view = [[FluctCInterface sharedObject] bannerWithID:object_id];
    if (view) {
        [view removeFromSuperview];
        return true;
    }
    return false;
}

#pragma mark - FluctInterstitialView

bool FluctInterstitialViewCreate(int object_id, char *media_id)
{
    if (![[FluctCInterface sharedObject] interstitialExistWithID:object_id]) {
        FSSInterstitialView *view = [[FSSInterstitialView alloc] init];
        if (media_id != NULL && strlen(media_id) > 0) {
            [view setMediaID:@(media_id)];
        }
        [[FluctCInterface sharedObject] setInterstitial:view forID:object_id];
        return true;
    }
    return false;

}

bool FluctInterstitialViewDestroy(int object_id)
{
    return [[FluctCInterface sharedObject] removeInterstitialForID:object_id];
}

bool FluctInterstitialViewExist(int object_id)
{
    return [[FluctCInterface sharedObject] interstitialExistWithID:object_id];
}

bool FluctInterstitialViewSetMediaID(int object_id, char *media_id)
{
    FSSInterstitialView *view = [[FluctCInterface sharedObject] interstitialWithID:object_id];
    if (view) {
        if (media_id != NULL && strlen(media_id) > 0) {
            [view setMediaID:@(media_id)];
            return true;
        }
    }
    return false;
}

bool FluctInterstitialViewShow(int object_id, char *hex_color_string)
{
    FSSInterstitialView *view = [[FluctCInterface sharedObject] interstitialWithID:object_id];
    if (view) {
        NSString *hexColorString = nil;
        if (hex_color_string != NULL && strlen(hex_color_string) > 0) {
            hexColorString = @(hex_color_string);
        }
        [view showInterstitialAdWithHexColor:hexColorString];
        return true;
    }
    return false;
}

bool FluctInterstitialViewDismiss(int object_id)
{
    FSSInterstitialView *view = [[FluctCInterface sharedObject] interstitialWithID:object_id];
    if (view) {
        [view dismissInterstitialAd];
        return true;
    }
    return false;
}

#pragma mark - FluctRewardedVideo
    void _FluctPlugin_RewardedVideo_Load(char *groupId, char *unitId, char *userId, int gender, char *birthday, int age)
    {
        if (userId != NULL || gender != 0 || birthday != NULL || age != 0)
        {
            NSString *userIdNullable = userId == NULL ? NULL : @(userId);
            NSString *birthdayNullable = birthday == NULL ? NULL : @(birthday);
            [[FluctCInterface sharedObject] loadFluctRewardedVideo:@(groupId) unitId:@(unitId) userId:userIdNullable gender:gender birthday:birthdayNullable age:age];
        } else {
            [[FluctCInterface sharedObject] loadFluctRewardedVideo:@(groupId) unitId:@(unitId)];
        }
    }

    void _FluctPlugin_RewardedVideo_Present(char *groupId, char *unitId)
    {
        [[FluctCInterface sharedObject] presentFluctRewardedVideo: @(groupId) unitId: @(unitId)];
    }

    bool _FluctPlugin_RewardedVideo_HasAdAvailable(char *groupId, char *unitId)
    {
        return [[FluctCInterface sharedObject] hasAdAvailableFluctRewardedVideo: @(groupId) unitId: @(unitId)];
    }

#ifdef __cplusplus
}
#endif


