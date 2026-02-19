from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.by import By
from webdriver_manager.chrome import ChromeDriverManager

import undetected_chromedriver as uc
import time

def get_page_html_stealth(url):
    options = uc.ChromeOptions()
    options.add_argument("--no-sandbox")
    options.add_argument("--disable-dev-shm-usage")
    scroll_pause_time = 3
    
    driver = uc.Chrome(options=options)

    try:
        driver.get(url)
        time.sleep(3)
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight / 3);")
        print("1/3")
        time.sleep(scroll_pause_time)        
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight * 2 / 3);")
        print("2/3")
        time.sleep(scroll_pause_time)
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
        print("Прокрутили до конца")
        time.sleep(scroll_pause_time)
        html_content = driver.page_source
        return html_content
        
    except Exception as e:
        print(f"Ошибка: {e}")
        return None
    finally:
        driver.quit()

FIRST_PAGE = 1
LAST_PAGE = 29
for page in range(FIRST_PAGE, LAST_PAGE + 1):
    i = 3 # retries
    while i > 0:
        try:
            url = f"https://www.mvideo.ru/melkaya-kuhonnaya-tehnika-3/elektrochainiki-96/f/category=elektricheskie-chainiki-482?page={page}"
            html = get_page_html_stealth(url)

            if html:
                filename = f"pages/page_{page}.html"
                with open(filename, "w", encoding="utf-8") as f:
                    f.write(html)
                print(f"  Сохранено в {filename}")
            i = 0
        except:
            i -= 1
