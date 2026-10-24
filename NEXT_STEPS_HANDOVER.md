# Handover & Next Steps: Room 404 Interactions & Visuals

**Ngày ghi nhận:** 2026-10-25 (00:40)  
**Tình trạng hiện tại:** Kiến trúc code tương tác (Component, Input lock, State Machine) đã chạy ổn định và pass test, nhưng **mặt visual / mỹ thuật / góc nhìn tư thế (perspective & art presentation) chưa đạt yêu cầu** theo feedback của user.

---

## 1. Các vấn đề cần chỉnh sửa (User Feedback)

### 🔴 Animation nằm nghỉ chưa đúng & Tư thế nằm chưa đúng
- **Hiện trạng:** Sprite nằm và chuỗi animation enter/exit tạo ra chưa đúng góc nhìn isometric của căn phòng (hướng nằm, tỷ lệ cơ thể và cách đặt lưng trên mặt đệm chưa tự nhiên).
- **Cần làm:** 
  - Xem xét lại góc chiếu 2.5D/isometric của mặt giường `bunk_bed_lengthwise`.
  - Thiết kế lại tư thế nằm (đầu quay về phía tường trong/gối, chân quay về phía trước/thành giường hoặc theo đúng trục giường).
  - Làm lại chuỗi frame chuyển tiếp trèo lên đệm sao cho hợp lý về mặt hình thể học trong không gian isometric.

### 🔴 Ngồi vào ghế chưa đúng
- **Hiện trạng:** Khi ngồi, tư thế nhân vật với chiếc ghế đỏ của bàn học (`study_desk_chairs.png`) chưa ăn khớp, pivot và độ cao ngồi chưa tự nhiên so với mặt ghế và mặt bàn.
- **Cần làm:**
  - Tinh chỉnh lại `SeatPivot`, sorting layer/order để phần thân dưới nằm sau mặt bàn nhưng ngồi đúng trên mặt đệm ghế.
  - Kiểm tra lại sprite ngồi (`luon_sitting_chair.png`) có khớp với dáng ngồi của ghế bàn học hay chưa.

### 🔴 Mở cửa chưa đúng & Mở cửa ra phải thấy hành lang bên ngoài
- **Hiện trạng:** Hiện tại cửa mở ra chỉ có mảng màu tối/đen phẳng (`DoorOpeningPixel.asset`), chưa thể hiện được không gian hành lang thực tế bên ngoài phòng KTX 404.
- **Cần làm:**
  - Thiết kế/bổ sung backdrop hành lang KTX (dormitory hallway) phía sau ô cửa: tường hành lang, ánh đèn huỳnh quang, lan can hoặc cửa đối diện.
  - Căn chỉnh lại góc mở và phép chiếu phối cảnh của cánh cửa (`HingedRoomDoor`) khi xoay vào trong.

---

## 2. Danh sách file liên quan cần tiếp tục xử lý

- **Environment & Backdrop cửa:**
  - `mobile-2d/Assets/Room404Layout/DoorOpeningSprite.asset`
  - `mobile-2d/Assets/Scripts/HingedRoomDoor.cs`
  - Backdrop hành lang (Hallway backdrop asset).
- **Bed & Chair Interaction:**
  - `mobile-2d/Assets/Scripts/ChairInteractable.cs` & `mobile-2d/Assets/Scripts/BedInteractable.cs`
  - `mobile-2d/Assets/Scripts/LuonCharacterVisual.cs`
  - `mobile-2d/Assets/Sprites/MVP/characters/bed_interaction/`
  - `mobile-2d/Assets/Animations/BedInteraction/`
- **Scene Setup & Pointers:**
  - `mobile-2d/Assets/Editor/RoomInteractionSceneSetup.cs`
  - `mobile-2d/Assets/Editor/MVPSceneBuilderFinal.cs`

---

## 3. Kế hoạch khi tiếp tục
1. Gom batch tạo asset hành lang bên ngoài cửa và tư thế nằm/ngồi chuẩn phối cảnh phòng 404.
2. Ghép nền hành lang phía sau ô cửa để khi mở cửa thấy ngay bên ngoài.
3. Chỉnh lại pivot ghế và giường để nhân vật khớp hoàn toàn với nội thất.
