# 家庭餐廳訂位

## 使用技術
**Client端: ASP.NET Core MVC、Bootstrap、jQuery、Ajax**  

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
  
- 進入「我要訂位頁」，從日期開始完成訂位，日期欄位顯示已預設為當天
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D1.png)  
<br/>
  
- 日期選擇限制從當天為開始，選完日期後點選「選好日期按我」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D2.png)  
<br/>
  
- 選完日期後顯示其他訂位資訊欄位
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D3.png)  
<br/>
  
- 時段下拉式選單顯示出當天開放訂位的時段，後方附有即時空位數
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D4.png)  
<br/>
  
- 因前2時段已過，不會再顯示於時段下拉式選單
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%816.png)  
<br/>
  
- 點選「按我訂位」以完成訂位
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D5.png)  
<br/>
  
- 若「兒童座椅需求數」大於「座位需求」，將出現錯誤訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A9%97%E8%AD%891.png)  
<br/>
  
- 若「訂位人數」大於當日「用餐時段」即時空位數，將出現錯誤訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%99%82%E6%AE%B5%E5%8D%B3%E6%99%82%E4%BA%BA%E6%95%B82.png)  
<br/>
  
- 訂位完成後將至訂位完成頁，顯示訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%90%E5%8A%9F1.png)  
<br/>
  
- 從首頁中，原14:30-16:00時段空位數也從原來45，改變成訂位後的42
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%812.png)
<br/>
  
- 若當日同一支連絡電話已經訂過位...
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A9%97%E8%AD%892.png)  
<br/>
  
- 第一次訂位成功後...
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A9%97%E8%AD%893.png)  
<br/>
  
- 再次訂位將出現錯誤訊息!
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A9%97%E8%AD%894.png)  
<br/>
  
- 當日時段若空位數剩餘10，將呈現黃色
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%813.png)  
<br/>
  
- 空位剩餘5，將呈現紅色
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%814.png)  
<br/>
  
- 無空位則呈現不開放訂位訊息
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%815.png)  
<br/>
  
- 我要訂位頁的時段下拉式選單也不會再出現無空位的時段
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%88%91%E8%A6%81%E8%A8%82%E4%BD%8D6.png)  
<br/>  

  
2. 查詢訂位資訊
  
- 點選「查詢/修改/取消訂位」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E9%A6%96%E9%A0%818.png)  
<br/>
  
- 點選「按我選擇日期」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/%E6%9F%A5%E8%A9%A2%E9%A0%812.png)  
<br/>
  
- 選完日期後將出現連絡電話欄位，輸入資訊後點選「按我查詢」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%9F%A5%E8%A9%A21.png)  
<br/>
  
- 查詢結果如圖紅框處所示
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%9F%A5%E8%A9%A22.png)  
<br/>
  
- 若訂位本身時間未過，就可以查到資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%9F%A5%E4%B8%8D%E5%88%B01.png)  
<br/>
  
- 但若時間過了...
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%9F%A5%E4%B8%8D%E5%88%B02.png)  
<br/>
  
- 查詢將出現錯誤訊息!
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E6%9F%A5%E4%B8%8D%E5%88%B03.png)  
<br/>  
  
  
3. 修改訂位資訊
  
- 在「訂位查詢頁」，點選「修改」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E4%BF%AE%E6%94%B95.png)  
<br/>
  
- 進入修改訂位頁修改訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E4%BF%AE%E6%94%B92.png)  
<br/>
  
- 修改完成後點選「儲存修改」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E4%BF%AE%E6%94%B93.png)  
<br/>
  
- 儲存修改後將導向至訂位完成頁顯示訂位資訊
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E4%BF%AE%E6%94%B94.png)  
<br/>  
  
  
4. 取消訂位
  
- 以時段17:30-19:00為例
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E5%8F%96%E6%B6%881.png)  
<br/>
  
- 在「訂位查詢頁」，點選「取消」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E5%8F%96%E6%B6%882.png)  
<br/>
  
- 在彈跳視窗中，點選「確定放棄」
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E5%8F%96%E6%B6%883.png)
<br/>
  
- 取消完成將回到首頁，上方出現取消成功訊息，人數也補回原時段
![](https://github.com/huaiminhu/Project_Reservation/blob/main/Pics/0111/%E5%8F%96%E6%B6%884.png)  
