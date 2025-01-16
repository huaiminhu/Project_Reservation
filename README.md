# 家庭餐廳訂位

## 使用技術
**Client端: ASP.NET Core MVC、Bootstrap、jQuery、AJAX**  

**Server端: ASP.NET Core Web API、Entity Framework、SQL SERVER**



## 軟體介紹  

### 一、頁面簡介  

> 訂位首頁   

![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP1.png)  
<br/>  
  
> 我要訂位頁  

![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP2.png)  
<br/>  
  
> 訂位完成頁  

![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP3.png)  
<br/>  
  
> 訂位查詢頁  

![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP4.png)  
<br/>  
  
> 修改訂位頁  

![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP5.png)  
<br/>  

  
### 二、功能介紹  
  
1. 訂位  

- 以時段13:00-14:30為例，點選「我要訂位」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP6.png)  
<br/>
  
- 進入「我要訂位頁」，從「訂位日期」欄位開始完成訂位，欄位內容顯示已預設日期為當天
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP7.png)  
<br/>
  
- 日期選擇限制從當天為開始
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP8.png)  
<br/>
  
- 「用餐時段」下拉式選單顯示出當天開放訂位的時段，後方附有即時空位數
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP9.png)  
<br/>

- 更換日期後，後方即時空位數也隨日期變動
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP10.png)  
<br/>
  
- 點選「按我訂位」以完成訂位
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP11.png)  
<br/>

- 訂位完成後將至「訂位完成頁」，顯示訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP12.png)  
<br/>

- 返回首頁後，13:00-14:30時段空位數從原來45，改變成訂位後的39
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP13.png)
<br/>

- 若「訂位人數」大於當日「用餐時段」即時空位數，將出現錯誤訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP14.png)  
<br/>
  
- 若「兒童座椅需求數」大於「訂位人數」，將出現錯誤訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP15.png)  
<br/>
  
- 訂位首頁中，時段空位數在10以內呈現黃色，5以內呈現紅色，沒有空位和已過時段呈現不開放訂位訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP15A1.png)  
<br/>
  
- 「我要訂位頁」的時段下拉式選單也不會再出現已過或沒有空位的時段
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP15A2.png)  
<br/>  

  
2. 查詢訂位資訊
  
- 點選「查詢/修改/取消訂位」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP16.png)  
<br/>
  
- 輸入「日期」和「連絡電話」欄位資訊後點選「按我查詢」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP17.png)  
<br/>
  
- 「按我查詢」下方呈現查詢結果
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP18.png)  
<br/>
  
- 若查詢不存在的訂位，將出現錯誤訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP19.png)  
<br/>

  
3. 修改訂位資訊
  
- 在「訂位查詢頁」查詢結果後方，點選「修改」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP20.png)  
<br/>
  
- 進入修改訂位頁修改訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP21.png)  
<br/>
  
- 修改完成後點選「儲存修改」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP22.png)  
<br/>
  
- 儲存修改後將導向至訂位完成頁顯示訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP23.png)  
<br/>  
  
  
4. 取消訂位

- 在「訂位查詢頁」查詢結果後方，點選「取消」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP24.png)  
<br/>
  
- 在彈跳視窗中，點選「確定放棄」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP25.png)
<br/>
  
- 取消完成將回到首頁，上方出現取消成功訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0116display/DP26.png)  
