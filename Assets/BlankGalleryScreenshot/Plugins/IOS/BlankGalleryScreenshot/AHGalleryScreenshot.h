
#import <Foundation/Foundation.h>

@interface AHGalleryScreenshot : NSObject

#ifdef __cplusplus

extern "C" {
#endif
    void addVideoToGallery(const char * path);
    void addImageToGallery(const char * path);
#ifdef __cplusplus
}
#endif

@end
