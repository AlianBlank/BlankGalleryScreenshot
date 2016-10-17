#import "AHGalleryScreenshot.h"

@implementation AHGalleryScreenshot

// 消息桥 名称
const char * GalleryScreenshotBridgeLink = "GalleryScreenshotBridgeLink";
// 状态改变的接受函数名称
const char * GalleryScreenshotStateChange = "GalleryScreenshotStateChange";

+ (void)addSaveImageToGallery:(const char *) path{
    UnitySendMessage(GalleryScreenshotBridgeLink,GalleryScreenshotStateChange,"Start");
    
    
    NSString * imagepath = [NSString stringWithUTF8String:path];
    
    NSLog(@"Source Image Path : %@",imagepath);
    
    if([[NSFileManager defaultManager] fileExistsAtPath:imagepath]){
        
        // 获取日历
        NSDateComponents * components = [[NSCalendar currentCalendar] components:NSCalendarUnitYear | NSCalendarUnitMonth | NSCalendarUnitDay | NSCalendarUnitHour | NSCalendarUnitMinute | NSCalendarUnitSecond | NSCalendarUnitNanosecond fromDate:[NSDate date]];
        NSInteger year = [components year];
        NSInteger month = [components month];
        NSInteger day = [components day];
        NSInteger hour = [components hour];
        NSInteger minute = [components minute];
        NSInteger second = [components second];
        NSInteger nanosecond = [components nanosecond];
        // 拼接文件名
        NSString * fileName = [NSString stringWithFormat:@"%ld%ld%ld%ld%ld%ld%ld",(long)year,month,day,hour,minute,second,nanosecond];
        
        // 拼接新图片文件的目录
        NSString * newImagePath =[NSString stringWithFormat:@"%@/%@.png",[NSHomeDirectory() stringByAppendingPathComponent:@"Documents"],fileName];
        //NSLog(@" 新文件路径 %@",newImagePath);
        // 复制文件到沙盒文档目录
        
        NSError *error;
        
        [[NSFileManager defaultManager] copyItemAtPath:imagepath toPath:newImagePath error:&error];
        if (error != nil) {
             NSLog(@" Error : %@",error);
        }
       
        imagepath = newImagePath;
    }else{
        NSLog(@"文件不存在！！！");
        return;
    }
    UIImage * image = [UIImage imageWithContentsOfFile:imagepath];
    if(image != nil)
    {
        NSLog(@" Start Write to Image!!! ");
        UIImageWriteToSavedPhotosAlbum(image, nil, NULL, NULL);
        UnitySendMessage(GalleryScreenshotBridgeLink,GalleryScreenshotStateChange,"Finish");
        NSLog(@" End Write to Image!!! ");
    }
    // 延迟3秒 删除新产生的文件
    [self performSelector:@selector(deleteImage:) withObject:imagepath afterDelay:3];
    }
+(void)deleteImage:(NSString *) imagepath{
    [[NSFileManager defaultManager] removeItemAtPath:imagepath error:nil];
}

void addImageToGallery(const char * path){
     [AHGalleryScreenshot addSaveImageToGallery:path];
}

@end
