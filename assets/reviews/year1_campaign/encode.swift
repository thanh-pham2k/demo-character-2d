import Foundation
import AppKit
import AVFoundation
import CoreVideo

// Usage: swift encode.swift <timestamped PNG directory> <output.mp4>
let input=URL(fileURLWithPath:CommandLine.arguments[1])
let output=URL(fileURLWithPath:CommandLine.arguments[2])
let rows=try String(contentsOf:input.appendingPathComponent("timestamps.csv"),encoding:.utf8).split(separator:"\n").dropFirst()
let duration=Double(try String(contentsOf:input.appendingPathComponent("duration.txt"),encoding:.utf8).trimmingCharacters(in:.whitespacesAndNewlines))!
if FileManager.default.fileExists(atPath:output.path){try FileManager.default.removeItem(at:output)}
let width=960,height=540
let writer=try AVAssetWriter(outputURL:output,fileType:.mp4)
let settings:[String:Any]=[AVVideoCodecKey:AVVideoCodecType.h264,AVVideoWidthKey:width,AVVideoHeightKey:height,
    AVVideoCompressionPropertiesKey:[AVVideoAverageBitRateKey:3500000,AVVideoExpectedSourceFrameRateKey:15,AVVideoProfileLevelKey:AVVideoProfileLevelH264MainAutoLevel]]
let track=AVAssetWriterInput(mediaType:.video,outputSettings:settings)
track.expectsMediaDataInRealTime=false
let adaptor=AVAssetWriterInputPixelBufferAdaptor(assetWriterInput:track,sourcePixelBufferAttributes:[kCVPixelBufferPixelFormatTypeKey as String:kCVPixelFormatType_32BGRA,kCVPixelBufferWidthKey as String:width,kCVPixelBufferHeightKey as String:height])
writer.add(track);writer.startWriting();writer.startSession(atSourceTime:.zero)
for row in rows {
    let cells=row.split(separator:",");let time=Double(cells[1])!
    while !track.isReadyForMoreMediaData {Thread.sleep(forTimeInterval:0.002)}
    try autoreleasepool {
        let data=try Data(contentsOf:input.appendingPathComponent(String(cells[0])))
        let image=NSImage(data:data)!.cgImage(forProposedRect:nil,context:nil,hints:nil)!
        var optional:CVPixelBuffer?
        CVPixelBufferCreate(kCFAllocatorDefault,width,height,kCVPixelFormatType_32BGRA,[kCVPixelBufferCGImageCompatibilityKey:true,kCVPixelBufferCGBitmapContextCompatibilityKey:true] as CFDictionary,&optional)
        let pixel=optional!;CVPixelBufferLockBaseAddress(pixel,[])
        let bitmap=CGBitmapInfo.byteOrder32Little.rawValue | CGImageAlphaInfo.premultipliedFirst.rawValue
        let context=CGContext(data:CVPixelBufferGetBaseAddress(pixel),width:width,height:height,bitsPerComponent:8,bytesPerRow:CVPixelBufferGetBytesPerRow(pixel),space:CGColorSpaceCreateDeviceRGB(),bitmapInfo:bitmap)!
        context.interpolationQuality = .none;context.draw(image,in:CGRect(x:0,y:0,width:width,height:height))
        CVPixelBufferUnlockBaseAddress(pixel,[])
        if !adaptor.append(pixel,withPresentationTime:CMTime(seconds:time,preferredTimescale:60000)){throw writer.error ?? NSError(domain:"capture",code:1)}
    }
}
writer.endSession(atSourceTime:CMTime(seconds:duration,preferredTimescale:60000));track.markAsFinished()
let done=DispatchSemaphore(value:0);writer.finishWriting{done.signal()};done.wait()
guard writer.status == .completed else {throw writer.error ?? NSError(domain:"capture",code:2)}
print("Encoded \(rows.count) actual frames, \(duration)s -> \(output.path)")
