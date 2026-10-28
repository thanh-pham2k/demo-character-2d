# Room 404 blockout — chưa duyệt

Ảnh chuẩn 1536 × 1024; render thử 768 × 512. Camera hiện dùng orthographic size 3.9, tỷ lệ 3:2, chưa khóa. Ảnh `Assets/Temp/room404-shell09.png` là render Edit Mode, không kiểm tra gameplay.

So sánh bounding box, tọa độ pixel 768 × 512, origin góc trái trên (mốc reference chỉ là ước lượng bằng mắt):

| Mốc | Reference ước lượng | Bounds renderer chiếu qua camera |
| --- | --- | --- |
| Cửa sổ | [239.5, 63, 398, 167.5] | [246, 66, 397, 169] |
| Cánh cửa | [435.5, 64, 535, 193] | [436, 63, 534, 187] |

Bounds reference bao gồm khung; bounds render chỉ gồm khối window hoặc panel door, nên số không phải phép đo tương đương tuyệt đối. Chưa có ngưỡng PASS. Room shell có tường vát và dầm trần thử, nhưng tường và giường đang giao nhau; cần chỉnh. Thang chưa nhìn rõ, khung cửa chỉ là blockout, cửa chưa có animation/tương tác, chưa xác nhận collision và lối đi. Player là capsule placeholder. Camera và structure checkpoint chưa PASS.

Cập nhật foreground: `Assets/Temp/room404-foreground12.png` hiển thị thảm, bàn giữa và hai tủ blockout. Thảm được đặt theo điểm chiếu xấp xỉ từ reference; bàn và tủ là prefab dùng lại. Vị trí tủ trên ảnh còn vào trong hơn reference; chưa thay đổi theo pixel nếu việc đó làm tủ giao tường. Camera, collision và gameplay vẫn chưa PASS.

## Checkpoint prototype (chưa kiểm tra Play Mode)

`Room404Prototype` tạo menu F8 trong bộ nhớ, không đọc hoặc ghi save chính. F8 → 1 giữa phòng, 2 bàn trái, 3 bàn phải, 4 thang trái, 5 thang phải, 6 cửa; WASD di chuyển, E thử tương tác gần anchor. Mỗi lựa chọn reset trạng thái pose và cửa. Đây chỉ là tương tác placeholder: ngồi/nằm đổi hình capsule, ngủ snap tới anchor; chưa có animation học/leo hay hệ thống gameplay thật. Cần nghiệm thu collision, khung cửa, vị trí đặt và pose sau khi structure/camera được duyệt.

Kiểm tra ngày 28/10: truy vấn `Physics.ComputePenetration` không thấy collider furniture hai bên giao hai tường bên; kiểm tra capsule tại năm vị trí tiếp cận checkpoint không thấy collider khác chồng vào (không tính sàn/player). Đây chỉ là phép kiểm tra hình học tại các điểm, không chứng minh đường đi. Play Mode khởi chạy, frameCount tăng; gọi trực tiếp hàm checkpoint bàn trái và tương tác đặt trạng thái ngồi, checkpoint cửa và tương tác đặt trạng thái mở, checkpoint giữa reset vị trí. Chưa xác nhận phím F8/1–6 hoặc E bằng input thực: lệnh mô phỏng phím của CLI chưa cho kết quả xác thực. Chưa kiểm tra khả năng đi bộ, leo, va chạm thực tế, pose hay occlusion. Camera chưa khóa.

Kiểm tra bàn phím thật trong Unity: F8 mở danh sách, phím 2 rồi E đưa placeholder tới ghế trái và vào pose ngồi; phím 4 rồi E đưa placeholder nằm trên giường trái; phím 6 rồi E mở panel cửa. Pose ngồi hiện chỉ thấy phần trên của capsule do ghế/bàn che, nên không đạt tiêu chí dễ đọc. Thao tác ngủ hiện snap trực tiếp, chưa có bước leo; cửa mở ra vùng chưa có môi trường phía sau. Chưa xác minh được đi xuyên cửa bằng WASD: lần giữ phím bị gián đoạn khi ứng dụng đổi trạng thái. Kết thúc Play Mode sau kiểm tra.

Cập nhật pose: offset ngồi tạm ra phía camera giúp capsule hiện rõ hơn trong Game View. F8 → 2 → E đã được thử bằng phím thật; đây vẫn là capsule thu nhỏ, chưa phải animation ngồi. F8 → 4 → E → E đã đưa placeholder trở về gần `ExitPoint` trái, tại x=-2.45, z=-0.70; chưa xác nhận đường leo hoặc pose nhân vật thật. Hai `ExitPoint` đã được nối vào component và checkpoint reset trạng thái tạm.

Cập nhật đường leo tạm: F8 → 4 → E chạy chuyển động placeholder qua ClimbPoint và lên SleepPoint; quan sát sau chuyển động cho `sleeping=True`, `transitioning=False`. E lần nữa đi qua ExitPoint và khôi phục CharacterController, vị trí (-2.45, 0.93, -0.70). Khi chuyển động, controller tạm tắt và transform được nội suy, nên chưa xác minh collision dọc đường; đây không phải animation leo thật. F8 reset vẫn cần kiểm tra trong lúc chuyển động và cần kiểm tra tương tự bên phải.

Thử phím thật F8 → 5 → E: placeholder đạt SleepPoint phải (4.00, 3.00, 1.00), trạng thái sleeping và hết chuyển động. Sau E → F8 → 1, trạng thái trở lại không ngủ/không chuyển động, controller bật, vị trí giữa phòng (0, 0.93, 0). Chuỗi được gửi liên tiếp để thử ngắt chuyển động; chưa đo chính xác thời điểm chọn checkpoint so với coroutine, nên chưa khẳng định đã kiểm tra ngắt giữa từng đoạn. Collision trong khi leo vẫn bị bỏ qua.

Viewport thử: `Room404CameraViewport` giữ projection 3:2 bằng cách giới hạn camera rect theo kích thước Game View. Trong Play Mode, component đọc 869 × 481 và đặt rect rộng khoảng 0.830 ở giữa; 869 × 0.830 / 481 ≈ 1.50. `Screen.width/height` khi đọc qua lệnh Editor trả 1742 × 1056, khác kích thước component đọc ở callback runtime, nên không dùng giá trị lệnh đó làm bằng chứng. Chưa kiểm tra resize runtime hoặc thiết bị thật, và camera position/projection vẫn chưa được duyệt hay khóa.

Sau khi chỉnh furniture: thang được thu vào, bàn và ghế dùng prefab rộng hơn, hai cụm bàn/ghế dịch ra ngoài 0.2 world units. Ảnh `Assets/Temp/room404-furniture13-3to2.png` được chụp sau khi đặt `camera.aspect=1.5`; ảnh `room404-furniture13.png` trước đó không dùng aspect này và không phải phép so sánh hợp lệ. Kiểm tra hình học tĩnh không thấy collider furniture chồng tường bên; capsule tại các vị trí tiếp cận thang/ghế không chồng collider khác (không tính sàn/player). Không suy ra đường đi hay gameplay PASS. Camera vẫn chưa khóa.

Study prototype: F8 → 2 → E → E bằng phím thật vào trạng thái ngồi/học, hướng theo StudyPoint trái (`seated=True`, `studying=True`). Đây chỉ là trạng thái và hướng capsule, không có animation hoặc gameplay học. Khi kiểm tra Game View Free Aspect xuất hiện hình cũ ở hai dải ngoài viewport và chữ cũ chồng lên: camera rect không xóa toàn bộ framebuffer. Cần sửa phần clear và kiểm tra lại trước khi xem viewport 3:2 là PASS.

Đã sửa clear vùng ngoài viewport bằng camera phụ runtime chỉ xóa màn hình, culling mask 0; GameplayCamera vẫn là camera gameplay duy nhất. Render Game View ở Free Aspect sau khi mở/đóng menu thử không còn thấy chữ hoặc hình scene cũ tại các dải ngoài viewport (`Assets/Temp/room404-clear-after.png`). Thao tác mở/đóng menu trong lần kiểm tra này gọi trực tiếp logic; chưa kiểm tra mọi kích thước màn hình hoặc thiết bị thật. Camera gameplay chưa được duyệt/khóa.

Chụp so sánh lặp lại bằng `AgentScripts/CaptureRoom404Comparison.cs`: đặt tạm rect đầy đủ và aspect 3:2 cho camera trong RenderTexture 768 × 512, rồi khôi phục trạng thái camera. Lần này ảnh đầu ra trùng từng pixel với ảnh chụp 3:2 gần nhất; scene vẫn clean. `Assets/Temp/room404-prototype-difference.png` là ảnh sai khác RGB trực tiếp so với reference thu nhỏ. Sai lệch màu trung bình [73.36,52.31,30.11] phản ánh cả blockout chưa có texture/ánh sáng, không phải tiêu chí PASS cho layout hoặc gameplay. Camera chưa khóa.
