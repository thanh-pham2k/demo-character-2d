# 🛠️ KẾ HOẠCH TRIỂN KHAI KỸ THUẬT: PHÒNG 404 (PRODUCTION ROADMAP)

> Baseline thực thi cập nhật 25/10/2026: [PRODUCTION_FLOW_BASELINE.md](PRODUCTION_FLOW_BASELINE.md) và [REMAINING_ASSET_INVENTORY.md](REMAINING_ASSET_INVENTORY.md). Khi có mâu thuẫn với bản thiết kế ban đầu dưới đây, dùng baseline cập nhật. Tiến độ asset không đồng nghĩa gameplay đã hoàn thành.
> Design được tách theo [story/README.md](README.md): [pillars](GAME_DESIGN_PILLARS.md), [systems](GAMEPLAY_SYSTEMS_PLAN.md), [transitions](GAMEPLAY_TRANSITIONS_PLAN.md) và [playtest gates](PLAYTEST_AND_ACCEPTANCE_PLAN.md). Các tuần 1–8, sơ đồ framework/ScriptableObject và HUD 4 thanh bên dưới là đề xuất ban đầu, không phải cam kết tiến độ hay chỉ dẫn tạo lại runtime; xem kiến trúc/trạng thái thật tại [PRODUCTION_CAMPAIGN_IMPLEMENTATION.md](PRODUCTION_CAMPAIGN_IMPLEMENTATION.md).
### Dự án: Ký Túc Xá 404 — 6 Năm Đại Học Bách Khoa
* **Engine:** Unity (2D / 2.5D URP)
* **Ngôn ngữ:** C# (.NET Standard)
* **Kiến trúc:** Event-driven, Data-driven (ScriptableObject), State Machine & Component-based
* **Mục tiêu:** Tối đa hóa khả năng tái sử dụng (Reusability), module hóa toàn bộ hệ thống từ Prototype ➔ Release.

---

## 🏛️ PHẦN 1: KIẾN TRÚC CODING & CÁC MODULE CỐT LÕI (CORE SYSTEMS)

Để tránh việc mỗi năm phải viết lại code mới, toàn bộ 45 Missions trong 6 năm sẽ chạy trên **5 Engine Framework dùng chung**:

```text
┌────────────────────────────────────────────────────────────────────────┐
│                         GAME MANAGER (Singleton)                       │
│    Quản lý Trạng thái Game (MainMenu, InGame, Paused, Dialog, MiniGame) │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │
    ┌───────────────────────────────┼───────────────────────────────┐
    ▼                               ▼                               ▼
┌───────────────────────┐   ┌───────────────────────┐   ┌───────────────────────┐
│     QUEST SYSTEM      │   │   INTERACTION & DIALOG│   │   ACADEMIC & TIME     │
│ - QuestManager        │   │ - Interactable (IInteract)│ - TimeManager (Lapse)   │
│ - QuestStep (Base)    │   │ - DialogueManager     │   │ - AcademicPortalSystem│
│ - QuestDatabase (SO)  │   │ - ChoiceBoxUI         │   │ - 4 Stat Trackers     │
└───────────────────────┘   └───────────────────────┘   └───────────────────────┘
                                    │
                                    ▼
┌───────────────────────────────────────────────────────────────────────┐
│                          MINIGAME FRAMEWORK                           │
│ - BaseMiniGame (Start, Update, Complete, Fail)                        │
│ - Type A: Quick Time Event (QTE) / Time Attack (Dọn phòng, giấu đồ)   │
│ - Type B: Desk Interaction (Cài Win, Căn lề Word, Bấm YES, Uninstall) │
│ - Type C: Timing & Rhythm (Spam Snooze, Bấm phím thuyết trình)         │
└───────────────────────────────────────────────────────────────────────┘
```

### 1.1. Chi tiết các Module kỹ thuật:
1. **`PlayerController2D` (Tái sử dụng):**
   * Di chuyển 8 hướng (WASD / Joystick cảm ứng cho mobile), chuẩn hóa vector.
   * Xử lý tương tác vật lý (Raycast 2D / Trigger phát hiện đồ vật có thể tương tác `[ E ]`).
   * Quản lý Animation Driver (Idle, Walk, Sit, Sleep, Hold Item).
2. **Production campaign (đang dùng):**
   * Định nghĩa 45 mission và event theo thứ tự trong `Assets/Resources/ProductionCampaign/missions.json`.
   * Runtime xác thực event/action/choice hiện tại trước khi ghi effect; xem [PRODUCTION_CAMPAIGN_IMPLEMENTATION.md](PRODUCTION_CAMPAIGN_IMPLEMENTATION.md).
   * Không tạo thêm 45 ScriptableObject hoặc một QuestManager song song chỉ dựa trên đề xuất kiến trúc ban đầu này.
3. **`TimeAndTransitionManager` (Chuyển giao thời gian):**
   * Xử lý Time-lapse đồng hồ (`23:40 ➔ 04:32`).
   * Quản lý chu kỳ Ngày / Đêm bằng 2D Light (Global Light chuyển từ vàng cam chiều ➔ trắng đèn tuýp đêm ➔ xanh mờ sáng sớm).
   * Thẻ tiến độ học vụ chuyển Học kỳ / Năm học.
4. **`MiniGameManager`:**
   * Một interface chuẩn `IMiniGame` cho phép kích hoạt cửa sổ minigame nổi (Pop-up Canvas UI), tạm dừng di chuyển của Player, xử lý logic thắng/thua rồi trả kết quả về `QuestManager`.

---

## 🎨 PHẦN 2: QUY HOẠCH ASSET (TỐI ĐA HÓA TÁI SỬ DỤNG)

### 2.1. Sprite Nhân vật (Characters):
Chỉ cần **1 bộ khung xương/kích thước chuẩn (32x48px pixel hoặc 128x128px stylized)**, dùng chung Animator Controller:
* **Lê Văn Lươn (Player):** 
  * *Trang phục 1 (Năm 1 - 5):* Dép tổ ong, quần lửng túi hộp, áo phông bụi bặm, cạp quần dắt tuốc-nơ-vít.
  * *Trang phục 2 (Năm 6):* Áo sơ mi trắng quần tây (đi bảo vệ đồ án và nhận bằng).
* **Ninh Đạm:** Áo sơ mi cài kín cổ, kính cận dày cộp, tay ôm cặp sách.
* **Trần Gia Bảo Long:** Áo khoác da sành điệu, tóc vuốt bóng, phụ kiện dây chuyền.
* **Phùng Canh Tộ:** Áo hoa nhí/thổ cẩm vintage, túi đeo chéo.
* **NPCs dùng chung (Generic):** Bác bảo vệ KTX (áo xanh), Cô quản lý KTX, Giảng viên Bách Khoa (áo vest, kính), Bạn nữ Crush, 2 Tân sinh viên năm nhất (thay thế giường năm 5).
* **Animation chung cho tất cả nhân vật:** `Idle`, `Walk` (4 hướng), `Sit` (ngồi học/ngồi ăn lẩu), `Sleep` (nằm giường).

### 2.2. Background & Môi trường (Environments):
Game gói gọn trong **4 Bản đồ (Scenes/Rooms)**:
1. **Phòng KTX 404 (Main Hub - 70% game):**
   * Kích thước 20m², tường vôi cũ, sàn gạch men, 2 giường tầng sắt đôi, 2 bàn học gỗ ép, quạt trần, cửa sổ nhìn ra Làng Đại học, cửa sắt có biển số 404.
   * *Biến đổi theo năm:* Năm 1 sạch sẽ; Năm 2 ngập rác mì tôm; Năm 3 dán biển `CÀI WIN 404`; Năm 5 đồ đạc của 3 người bạn biến mất, thay bằng đồ của người lạ; Năm 6 trống trơn hoàn toàn khi Lươn tắt đèn.
2. **Hành lang & Sảnh KTX Khu B:**
   * Dãy phòng kéo dài, cầu thang bộ tầng 4, bảng thông báo KTX, góc ban công gọi điện thoại về quê.
3. **Giảng đường Bách Khoa (H6 & Phòng Hội Đồng):**
   * Dãy bàn ghế dốc, bục giảng viên, máy chiếu, bảng phấn.
4. **Quán Xá Sinh Viên / Bờ Hồ Đá (Ngoại cảnh):**
   * Bàn nhựa vỉa hè, đĩa bánh tráng nướng, ly trà đào, mặt nước Hồ Đá lộng gió.

### 2.3. Audio & Music (Âm thanh gợi cảm xúc):
* **SFX sinh viên:** Tiếng gõ phím cơ *lạch cạch*, tiếng quạt trần kẽo kẹt, tiếng nước lẩu sôi *sùng sục*, tiếng dép tổ ong quẹt trên sàn gạch, tiếng còi tàu xe buýt 33, tiếng chuông báo thức *reng reng*, tiếng click chuột đanh gọn.
* **BGM (Nhạc nền):**
  * *Năm 1 - 2:* Giai điệu Lo-fi sinh viên vui nhộn, tươi sáng, tiết tấu nhanh khi chơi game.
  * *Năm 3 - 4:* Tiết tấu lắng xuống, tiếng mưa rơi trên mái tôn KTX, giai điệu ghi-ta mộc mạc cô đơn.
  * *Năm 5 - 6:* Đoạn cao trào violin + piano lúc làm đồ án và bảo vệ thành công.
  * *Ending Credit:* Bài hát acoustic thanh xuân da diết.

---

## 💻 PHẦN 3: HỆ THỐNG UI & MÀN HÌNH TƯƠNG TÁC (SCREENS)

1. **HUD Gameplay chính (In-game Minimalist UI):**
   * Góc trên trái: Ô hiển thị Thời gian (Thứ, Giờ, Học kỳ, Năm).
   * Trạng thái học vụ là phần tóm tắt theo ngữ cảnh; Energy và Money chỉ xuất hiện khi đang tạo lựa chọn. Quan hệ ưu tiên thể hiện qua hành vi/thoại của NPC, không bắt buộc dùng thanh Bromance.
   * Góc dưới trái: Tracker mục tiêu nhiệm vụ hiện tại (Quest Objective).
   * Bong bóng phím bấm tương tác trên đầu đồ vật: `[ E ] Tương tác`.
2. **Màn hình Điện Thoại Sinh Viên (Smartphone UI):**
   * Bấm `[ Tab ]` để mở điện thoại của Lươn: Xem app Zalo nhóm "Phòng 404", xem Portal trường Bách Khoa, nhận tin nhắn khách cài Win, xem số dư tài khoản ngân hàng.
3. **Màn hình Laptop / Minigame Screen:**
   * Giao diện mô phỏng màn hình máy tính: Màn hình cài Windows, cửa sổ soạn thảo Word căn lề, màn hình IDE gõ code & debug, pop-up `League of Legends: PLAY?` và thanh giữ chuột `UNINSTALL`.
4. **Màn hình Học Vụ (Academic Status Screen):**
   * Hiện bảng điểm môn học cuối kỳ (A, B, C, D, F) và thông báo Cảnh cáo học vụ / Đủ điều kiện làm đồ án.

---

## 📅 PHẦN 4: LỘ TRÌNH TRIỂN KHAI THEO NĂM & CÁC MILESTONES (PRODUCTION ROADMAP)

Dự án được chia thành **6 Giai đoạn (Milestones)** rõ ràng:

```text
MILESTONE 0: Core Architecture & Prototype (Tuần 1 - 2)
     ↓
MILESTONE 1: Năm 1 — Tân Sinh Viên & Setup KTX (Tuần 3)
     ↓
MILESTONE 2: Năm 2 — Bẫy Ăn Chơi & Rớt Môn (Tuần 4)
     ↓
MILESTONE 3: Năm 3 — Mưu Sinh Cài Win & Tình Cảm (Tuần 5)
     ↓
MILESTONE 4: Năm 4 & Năm 5 — Tụt Lại, Người Ở Lại & Thức Tỉnh (Tuần 6)
     ↓
MILESTONE 5: Năm 6 — Đồ Án Tốt Nghiệp, Bảo Vệ & Hạ Màn (Tuần 7)
     ↓
MILESTONE 6: Polish, Sound, Balance & Release (Tuần 8)
```

---

### 🚀 CHI TIẾT CÔNG VIỆC TỪNG MILESTONE:

#### 🔹 MILESTONE 0: NỀN TẢNG CỐT LÕI (PROTOTYPE FRAMEWORK)
* **Coding:**
  * Dựng `PlayerController2D` (di chuyển, lật mặt, tương tác vật lý).
  * Xây dựng `QuestManager` và cấu trúc ScriptableObject cho nhiệm vụ.
  * Xây dựng `DialogueSystem` hiển thị khung thoại và avatar 4 nhân vật.
  * Xây dựng `TimeManager` hỗ trợ đổi ánh sáng Ngày/Đêm và Time-lapse.
* **Asset:** Dùng placeholder sprites cơ bản để test di chuyển và va chạm.
* **Kiểm thử:** Điều khiển nhân vật chạy quanh phòng, bấm `[ E ]` nói chuyện với NPC và kích hoạt hoàn thành 1 Quest mẫu.

---

#### 🔹 MILESTONE 1: NĂM 1 — TÂN SINH VIÊN (MISSIONS 01 ➔ 08)
* **Coding Features:**
  * Hệ thống né chướng ngại vật (Mission 01: Né xe ôm dù).
  * Minigame dọn phòng KTX đếm ngược 60 giây (Mission 04).
  * Minigame chọn món cơm sinh viên trong ngân sách 35k (Mission 05).
  * Logic puzzle toán mẹo trên giảng đường Giải Tích 1 (Mission 06).
* **Asset & Map:**
  * Dựng Scene Phòng 404 (bản sạch sẽ ban đầu), Scene Trạm xe buýt, Scene Giảng đường H6.
  * Sprite 4 nhân vật Lươn, Đạm, Long, Tộ lúc mới nhập học.
* **Kiểm thử Milestone 1:** Chơi mượt mà từ lúc xuống xe buýt đến bữa lẩu gắn kết cuối năm nhất, chuyển sang Năm 2 ổn định.

---

#### 🔹 MILESTONE 2: NĂM 2 — BẪY ĂN CHƠI & RỚT MÔN (MISSIONS 09 ➔ 16)
* **Coding Features:**
  * Minigame F5 giật slot môn học lúc 8h59.
  * Cơ chế Time-lapse: Bấm nút `[ YES ]` lúc 23:40 ➔ Đồng hồ tua đến 04:32 sáng.
  * Cơ chế Spam phím `SNOOZE` lúc 06:30 ➔ Ngủ quên đến 11h47.
  * Minigame Stealth 10s: Nghe tiếng bảo vệ giấu nồi lẩu vào tủ quần áo.
  * Màn hình công bố điểm: Hiện chữ `F` đỏ chót, kích hoạt cờ `HasAcademicWarning` và khóa môn tiên quyết.
* **Asset:**
  * Prefab rác vỏ mì tôm, lon nước ngọt tích tụ trong phòng 404.
  * Sound effect tiếng chìa khóa bảo vệ, tiếng gõ phím cơ dồn dập.
* **Kiểm thử Milestone 2:** Đảm bảo chuỗi nhân quả hoạt động hoàn hảo: Bấm YES ➔ Ngủ quên ➔ Cúp tiết ➔ Thi cắn bút ➔ Nhận điểm F và mất môn tiên quyết.

---

#### 🔹 MILESTONE 3: NĂM 3 — MƯU SINH CÀI WIN & TÌNH CẢM (MISSIONS 17 ➔ 23)
* **Coding Features:**
  * Minigame Cài Win 50k: Cắm USB Boot, click chuột chọn phân vùng, nhận tiền cộng vào tài khoản.
  * Bảng dịch vụ Side Jobs dán trước cửa phòng 404 (nhận đơn sửa máy).
  * Dialogue tree tương tác với crush và bạn trai của crush.
* **Asset & Map:**
  * Map Bờ Hồ Đá (ghế đá, mặt hồ, quầy trà đào bánh tráng nướng).
  * Sprite bạn nữ crush và anh bạn trai đi xe tay ga.
* **Kiểm thử Milestone 3:** Hệ thống kiếm tiền hoạt động trơn tru; sau khi thất tình cả 4 đứa ra Hồ Đá ngồi uống nước tạo đúng cảm giác trầm lắng.

---

#### 🔹 MILESTONE 4: NĂM 4 & 5 — TỤT LẠI, NGƯỜI Ở LẠI & THỨC TỈNH (MISSIONS 24 ➔ 36)
* **Coding Features:**
  * Logic chặn đăng ký đồ án tốt nghiệp cuối Năm 4 do thiếu tín chỉ.
  * Cơ chế thay đổi NPC phòng 404: Long, Tộ, Đạm rời KTX ➔ Spawn tân sinh viên lạ mặt dọn vào.
  * **Cơ chế giữ chuột UNINSTALL:** Bắt người chơi đè chuột trái chạy thanh % từ 0 đến 100%.
  * Chuyển trạng thái Lươn: 06:30 báo thức reo ➔ Bấm `[ W ]` bật dậy ngay (Không còn nút Snooze).
  * Chuỗi thi trả nợ môn: Pass Giải Tích 2 ➔ Mở khóa điều kiện đồ án năm 6.
* **Asset:**
  * Biến thể phòng 404 năm thứ 5: Giường của bạn bè có chăn màn lạ, không khí xa lạ.
  * UI thanh tiến trình Uninstall game.
* **Kiểm thử Milestone 4:** Kiểm tra cảm xúc người chơi lúc bạn bè dọn đi và khoảnh khắc xóa game tự đứng dậy trả nợ môn.

---

#### 🔹 MILESTONE 5: NĂM 6 — ĐỒ ÁN TỐT NGHIỆP, BẢO VỆ & HẠ MÀN (MISSIONS 37 ➔ 45)
* **Coding Features:**
  * Toàn bộ chuỗi gameplay đồ án tốt nghiệp: Nghiên cứu ➔ Code ➔ Demo bị bác bỏ ➔ Căn lề Word ➔ Fix bug 23h45.
  * Minigame Thuyết trình bảo vệ đồ án trước Hội đồng: Chuyển slide, giữ thanh tự tin, trả lời chất vấn Q&A ➔ Nhận điểm A.
  * Tương tác cuối cùng: Bấm phím **`[ E ] Tắt đèn phòng 404`** (Tắt đèn tuýp 2D Light, đóng cửa sắt).
  * Scene Lễ tốt nghiệp và Chầu nhậu Bến Bình Đông kết thúc game.
* **Asset:**
  * Sprite Lươn mặc áo sơ mi trắng cử nhân.
  * Sprite bố mẹ Lươn từ quê vào ôm hoa.
  * Sprite Đạm, Long, Tộ mặc đồ công sở chỉnh tề.
  * Map Bến Bình Đông (bờ kênh, bàn nhậu bia đêm).
* **Kiểm thử Milestone 5:** Toàn bộ arc tốt nghiệp chạy liền mạch, cuộc gọi cho mẹ xúc động, hành động tắt đèn và kết thúc game đọng lại dư vị sâu lắng.

---

#### 🔹 MILESTONE 6: POLISH, AUDIO, BALANCE & RELEASE
* Cân bằng nhịp độ chơi: Đảm bảo toàn bộ 45 Missions hoàn thành vừa vặn trong **60 – 90 phút**.
* Lồng ghép toàn bộ Sound Effects và Music Soundtrack theo từng giai đoạn.
* Tối ưu hóa hiệu năng, đóng gói bản build PC (Windows) và Mobile (Android/iOS).

---

## 📋 BẢNG DANH MỤC DATA / SCRIPTABLE OBJECT CẦN TẠO SẴN

```text
Assets/
├── Data/
│   ├── ProductionCampaign/  # Nguồn campaign hiện tại nằm trong Resources/ProductionCampaign/missions.json
│   ├── Dialogues/           # Toàn bộ hội thoại theo từng Mission (Text, Audio, Avatars)
│   ├── MiniGames/           # Config thông số các minigame (thời gian, phím bấm, độ khó)
│   └── AcademicPortal/      # Dữ liệu môn học, điều kiện tiên quyết, tín chỉ từng kỳ
```

Đây là phân nhóm nội dung ở mức khái niệm; không cam kết các thư mục hoặc định dạng này đã tồn tại. Với campaign, dùng nguồn JSON và runtime hiện hành thay vì bản lưu trữ ScriptableObject trong sơ đồ cũ.

Đây là blueprint khởi đầu, không phải bằng chứng mọi hệ thống hoặc mission đã được triển khai. Dùng production gates và trạng thái thực tế trong [PLAYER_EXPERIENCE_PLAN.md](PLAYER_EXPERIENCE_PLAN.md) cùng [PRODUCTION_CAMPAIGN_IMPLEMENTATION.md](PRODUCTION_CAMPAIGN_IMPLEMENTATION.md) để quyết định công việc tiếp theo.
