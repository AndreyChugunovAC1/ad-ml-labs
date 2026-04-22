import re

with open("all_linkns.txt", "w") as fo:
    for i in range(1, 29 + 1):
        with open(f"pages/page_{i}.html", "r") as f:
            html = f.read()
            pattern = r'href\s*=\s*["\']([^"\']+)["\']'
            links = re.findall(pattern, html, re.IGNORECASE)
            links = [x for x in links if re.match(r"^.*products/[^\/]+$", x)]
            links = list(set(links))
            for li in links:
                fo.write(f"{li}\n")